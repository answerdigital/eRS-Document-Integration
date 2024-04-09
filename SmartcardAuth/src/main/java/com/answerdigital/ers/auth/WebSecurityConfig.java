package com.answerdigital.ers.auth;

import com.answerdigital.ers.logging.LoggingInterceptor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.*;
import org.springframework.http.client.BufferingClientHttpRequestFactory;
import org.springframework.http.client.ClientHttpRequestFactory;
import org.springframework.http.client.ClientHttpRequestInterceptor;
import org.springframework.http.client.SimpleClientHttpRequestFactory;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.oauth2.client.userinfo.DefaultOAuth2UserService;
import org.springframework.security.oauth2.client.userinfo.OAuth2UserRequest;
import org.springframework.security.oauth2.client.userinfo.OAuth2UserService;
import org.springframework.security.oauth2.core.OAuth2AccessToken;
import org.springframework.security.oauth2.core.user.DefaultOAuth2User;
import org.springframework.security.oauth2.core.user.OAuth2User;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.util.CollectionUtils;
import org.springframework.web.client.RestTemplate;

import java.util.*;
import java.util.stream.Collectors;

@Configuration
@EnableWebSecurity
public class WebSecurityConfig {

    Logger logger = LoggerFactory.getLogger(WebSecurityConfig.class);

    private String orgId = System.getenv("ODS_CODE");

    /*
    In order to use this endpoint you must be an authenticated e-RS user or application and use one of the following e-RS roles:
    SERVICE_PROVIDER_CLINICIAN - R0050
    SERVICE_PROVIDER_CLINICIAN_ADMIN - R5170
     */
    private List<String> userRoleCodes = Arrays.asList("R0050", "R5170", "R8008");
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http
                .authorizeHttpRequests((requests) -> requests
                        .antMatchers("/", "/home", "/error", "/oauth2/authorization/**", "/webjars/**", "/assets/**", "/js/**", "/css/**", "/docs/**").permitAll()
                        .antMatchers("/handover").hasRole("ERS_ADMIN_USER")
                        .anyRequest().authenticated()
                )
                .oauth2Login().userInfoEndpoint().userService(this.oAuth2UserService());

        return http.build();
    }

    private OAuth2UserService<OAuth2UserRequest, OAuth2User> oAuth2UserService() {
        final DefaultOAuth2UserService delegate = new DefaultOAuth2UserService();

        return (userRequest) -> {
            RestTemplate userInfoTemplate = null;
            if (logger.isDebugEnabled()){
                ClientHttpRequestFactory httpFactory = new BufferingClientHttpRequestFactory(new SimpleClientHttpRequestFactory());
                userInfoTemplate = new RestTemplate(httpFactory);
                List<ClientHttpRequestInterceptor> interceptors = userInfoTemplate.getInterceptors();
                if (CollectionUtils.isEmpty(interceptors)) {
                    interceptors = new ArrayList<>();
                }
                interceptors.add(new LoggingInterceptor());
                userInfoTemplate.setInterceptors(interceptors);
            } else {
                userInfoTemplate = new RestTemplate();
            }
            //TODO: add logout behaviour
            //userInfoTemplate.setErrorHandler(delegate.);
            delegate.setRestOperations(userInfoTemplate);
            OAuth2User user = delegate.loadUser(userRequest);
            OAuth2AccessToken accessToken = userRequest.getAccessToken();

            HttpHeaders headers = new HttpHeaders();
            headers.setBearerAuth(accessToken.getTokenValue());
            headers.setAccept(Collections.singletonList(MediaType.APPLICATION_JSON));
            HttpEntity<Object> requestEntity = new HttpEntity<>(null, headers);
            String userInfoUrl = userRequest.getClientRegistration().getProviderDetails().getUserInfoEndpoint().getUri();
            ResponseEntity<Cis2User> userInfoResponse = userInfoTemplate.exchange(userInfoUrl, HttpMethod.GET, requestEntity, Cis2User.class);
            Cis2User cis2User = userInfoResponse.getBody();
            Set<GrantedAuthority> mappedAuthorities = new HashSet<>();

            if (cis2User.getRoles().stream().anyMatch(
                    cis2RbacRole ->
                            orgId.equals(cis2RbacRole.getOrgCode()) &&
                            //user userRoleCode has the format {Staff Group Code}:{Staff Sub Group Code}:{Job Role Code} e.g. S0040:G1010:R9605
                            userRoleCodes.contains(
                                    cis2RbacRole.getRoleCode().split(":")[cis2RbacRole.getRoleCode().split(":").length-1]
                            )
            )){
                mappedAuthorities.add(new SimpleGrantedAuthority("ROLE_ERS_ADMIN_USER"));
            }
            logger.debug("User Selected Role: {}", cis2User.getSelectedRoleId());
            logger.debug("Roles: {}", cis2User.getRoles().stream().map(Cis2RbacRole::getRoleCode).collect(Collectors.joining(", ")));

            String usernameAttribute = userRequest.getClientRegistration()
                    .getProviderDetails().getUserInfoEndpoint().getUserNameAttributeName();
            user = new DefaultOAuth2User(mappedAuthorities, user.getAttributes(), usernameAttribute);

            return user;
        };
    }
}
