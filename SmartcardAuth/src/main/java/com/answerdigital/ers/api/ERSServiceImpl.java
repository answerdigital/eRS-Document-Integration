package com.answerdigital.ers.api;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Component;
import org.springframework.web.client.HttpClientErrorException;
import org.springframework.web.client.HttpServerErrorException;
import org.springframework.web.client.RestTemplate;

import java.io.IOException;
@Component
public class ERSServiceImpl implements ERSService{
    @Value("${ers.session-handover-endpoint}")
    private String endpoint;
    @Override
    public AuthenticatedSessionResponse handover(AuthenticatedSession authenticatedSession) throws IOException {
        RestTemplate restTemplate = new RestTemplate();
        String sessionHandoverUrl = endpoint;

        HttpEntity<AuthenticatedSession> request = new HttpEntity<>(authenticatedSession);
        ResponseEntity<AuthenticatedSessionResponse> response = null;
        try {
            response = restTemplate.exchange(sessionHandoverUrl, HttpMethod.POST, request, AuthenticatedSessionResponse.class);
        } catch (HttpClientErrorException | HttpServerErrorException e){
            throw new IOException("HTTP error " + e.getRawStatusCode());
        }

        return response.getBody();
    }
}
