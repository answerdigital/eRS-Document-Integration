package com.answerdigital.ers.logging;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpRequest;
import org.springframework.http.client.ClientHttpRequestExecution;
import org.springframework.http.client.ClientHttpRequestInterceptor;
import org.springframework.http.client.ClientHttpResponse;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.util.stream.Collectors;

public class LoggingInterceptor implements ClientHttpRequestInterceptor {
    static Logger LOGGER = LoggerFactory.getLogger(LoggingInterceptor.class);

    @Override
    public ClientHttpResponse intercept(HttpRequest request, byte[] body, ClientHttpRequestExecution execution) throws IOException {
        LOGGER.debug("Request Body: {}", new String(body, StandardCharsets.UTF_8));
        ClientHttpResponse response = execution.execute(request, body);
        if (LOGGER.isDebugEnabled()){
            InputStreamReader inputStreamReader = new InputStreamReader(response.getBody(), StandardCharsets.UTF_8);
            String responseBody = new BufferedReader(inputStreamReader).lines().collect(Collectors.joining("\n"));
            LOGGER.debug("Response body: {}", responseBody);
        }
        return response;
    }
}
