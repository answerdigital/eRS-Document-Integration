package com.answerdigital.ers.auth;

import com.answerdigital.ers.api.AuthenticatedSession;
import com.answerdigital.ers.api.AuthenticatedSessionResponse;
import com.answerdigital.ers.api.ERSService;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Configuration;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.security.core.Authentication;
import org.springframework.security.oauth2.client.*;
import java.io.IOException;
import java.time.Duration;
import java.util.HashMap;
import java.util.Map;
import org.slf4j.Logger;
import org.springframework.security.oauth2.core.user.DefaultOAuth2User;
import org.springframework.security.oauth2.core.user.OAuth2User;

@Configuration
@EnableScheduling
public class RefreshTokenServiceImpl implements RefreshTokenService {

    Logger logger = LoggerFactory.getLogger(RefreshTokenServiceImpl.class);

    @Autowired
    private OAuth2AuthorizedClientService clientService;

    private Map<String, Authentication> currentClients = new HashMap<>();

    private static final String DEFAULT_CLIENT_REGISTRATION_ID = "cis2";

    @Autowired
    private ERSService ersService;

    @Override
    public void put(OAuth2AuthorizedClient client, Authentication principal){
        currentClients.put(
                principal.getName(),
                principal
        );
        clientService.saveAuthorizedClient(client, principal);
    }

    @Scheduled(cron = "0 0/1 * * * ?") //(cron = "0 0/5 * * * ?") //every 5 min
    public void reauthorizeCurrentClients() {
        logger.debug("Reauthorizing {} clients", currentClients.size());
        for (Map.Entry<String, Authentication> entry : currentClients.entrySet()) {
            String key = entry.getKey();
            Authentication value = entry.getValue();
            logger.debug("Reauthorizing client " + key);

            OAuth2AuthorizedClient reauthorizedClient = clientService.loadAuthorizedClient(DEFAULT_CLIENT_REGISTRATION_ID, key);
            RefreshTokenOAuth2AuthorizedClientProvider authorizedClientProvider = new RefreshTokenOAuth2AuthorizedClientProvider();
            //authorizedClientProvider.setClockSkew(Duration.ofMinutes(5)); // renew 5 minutes before expiry
            authorizedClientProvider.setClockSkew(Duration.ofMinutes(10));
            OAuth2AuthorizationContext authorizationContext = OAuth2AuthorizationContext.withAuthorizedClient(reauthorizedClient).principal(value).build();
            reauthorizedClient = authorizedClientProvider.authorize(authorizationContext);


            if (reauthorizedClient != null){
                logger.info("Reauthorized client {}", key);
                logger.debug("new access token: {}", reauthorizedClient.getAccessToken().getTokenValue());
                logger.debug("new refresh token: {}", reauthorizedClient.getRefreshToken().getTokenValue());
                AuthenticatedSession session = new AuthenticatedSession(
                        reauthorizedClient.getAccessToken().getTokenValue(),
                        key,
                        ((OAuth2User)value.getPrincipal()).getAttribute("name")
                );
                AuthenticatedSessionResponse response = null;
                clientService.saveAuthorizedClient(reauthorizedClient, value);
                try {
                    response = ersService.handover(session);
                } catch (IOException e){
                    logger.warn("Exception while handing over reauthorized session", e);
                }
                if (response == null) {
                    logger.warn("Unable to hand over reauthorized session for client {}", key);
                }
            } else {
                logger.info("Reauthorizing client {} failed as token not available or access not expired", key);
                //continue
            }
        }
    }
}