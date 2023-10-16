package com.answerdigital.ers.auth;

import org.springframework.security.core.Authentication;
import org.springframework.security.oauth2.client.OAuth2AuthorizedClient;

public interface RefreshTokenService {
    void put(OAuth2AuthorizedClient client, Authentication principal);
}
