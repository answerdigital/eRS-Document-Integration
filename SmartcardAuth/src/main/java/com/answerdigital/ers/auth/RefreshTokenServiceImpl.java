package com.answerdigital.ers.auth;

import com.answerdigital.ers.api.AuthenticatedSession;
import com.answerdigital.ers.api.AuthenticatedSessionResponse;
import com.answerdigital.ers.api.ERSService;
import com.answerdigital.ers.api.ERSServiceImpl;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.oauth2.client.*;
import org.springframework.security.oauth2.client.registration.ClientRegistration;
import org.springframework.security.oauth2.client.registration.ClientRegistrationRepository;
import org.springframework.security.oauth2.client.registration.InMemoryClientRegistrationRepository;
import org.springframework.security.oauth2.client.registration.ReactiveClientRegistrationRepository;
import org.springframework.security.oauth2.core.AuthorizationGrantType;

import javax.annotation.PostConstruct;
import java.io.IOException;
import java.time.Duration;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import org.slf4j.Logger;

@Configuration
@EnableScheduling
public class RefreshTokenServiceImpl {

    Logger logger = LoggerFactory.getLogger(RefreshTokenServiceImpl.class);

    @Autowired
    private ClientRegistrationRepository clientRegistrationRepository;
//    @Autowired
//    private OAuth2AuthorizedClientManager authorizedClientManager;
    @Autowired
    private OAuth2AuthorizedClientService clientService;

    private Map<String, Object> currentClients = new HashMap<>();

    private static final String DEFAULT_CLIENT_REGISTRATION_ID = "cis2";

    private ERSService ersService = new ERSServiceImpl(); //TODO: inject this




//    private static final OAuth2AuthorizeRequest DEFAULT_AUTHORIZE_REQUEST =
//            OAuth2AuthorizeRequest.withClientRegistrationId(DEFAULT_CLIENT_REGISTRATION_ID)
//                    .principal(DEFAULT_CLIENT_ID)
//                    .build();

    public void put(OAuth2AuthorizedClient client, Authentication principal){
        currentClients.put(principal.getName(), client.getRefreshToken().getExpiresAt());
        clientService.saveAuthorizedClient(client, principal);
    }

    @Scheduled(cron = "0 * * * * *") //every 1 min
    public void reauthorizeCurrentClients() {
        logger.debug("Reauthorizing {} clients", currentClients.size());
        for (Map.Entry<String, Object> entry : currentClients.entrySet()) {
            String key = entry.getKey();
            Object value = entry.getValue();
            logger.debug("Reauthorizing client " + key);

            OAuth2AuthorizedClient reauthorizedClient = clientService.loadAuthorizedClient(DEFAULT_CLIENT_REGISTRATION_ID, key);


//            OAuth2AuthorizeRequest request = OAuth2AuthorizeRequest.withClientRegistrationId(DEFAULT_CLIENT_REGISTRATION_ID)
//                    .principal(key)
//                    .build()
//            OAuth2AuthorizedClient reauthorizedClient = authorizedClientManager.authorize(request);

            if (reauthorizedClient != null){
                AuthenticatedSession session = new AuthenticatedSession(reauthorizedClient.getAccessToken().getTokenValue(), key, "TODO");
                AuthenticatedSessionResponse response = null;
                try {
                    response = ersService.handover(session);
                } catch (IOException e){
                    logger.warn("Exception while handing over session", e);
                }
                if (response != null) {
                    logger.warn("Unable to hand over session for client {}", key);
                }
            }
        }
    }




//    //@Bean
//    public ClientRegistrationRepository clientRegistrationRepository() {
//        return new InMemoryClientRegistrationRepository(ClientRegistration.withRegistrationId(DEFAULT_CLIENT_REGISTRATION_ID)
//                .authorizationGrantType(AuthorizationGrantType.CLIENT_CREDENTIALS)
//                .clientId(DEFAULT_CLIENT_ID)
//                .clientSecret("clientSecret")
//                .tokenUri("oauth2Uri")
//                .scope(List.of("scope1", "scope2"))
//                .build());
//    }

//    @Bean
//    public OAuth2AuthorizedClientManager authorizedClientManager(ClientRegistrationRepository clientRegistrationRepository) {
//
//        OAuth2AuthorizedClientProvider authorizedClientProvider =
//                OAuth2AuthorizedClientProviderBuilder.builder()
//                        .clientCredentials(clientCredentials ->
//                                // NOTE: Set a higher clock skew to force early token renewal
//                                clientCredentials.clockSkew(Duration.ofMinutes(5)))
//                        .build();
//
//        InMemoryOAuth2AuthorizedClientService authorizedClientService =
//                new InMemoryOAuth2AuthorizedClientService(clientRegistrationRepository);
//
//        AuthorizedClientServiceOAuth2AuthorizedClientManager authorizedClientManager =
//                new AuthorizedClientServiceOAuth2AuthorizedClientManager(
//                        clientRegistrationRepository, authorizedClientService);
//        authorizedClientManager.setAuthorizedClientProvider(authorizedClientProvider);
//
//        return authorizedClientManager;
//    }
}