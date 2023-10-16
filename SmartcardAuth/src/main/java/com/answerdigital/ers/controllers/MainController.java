package com.answerdigital.ers.controllers;

import com.answerdigital.ers.api.*;

import com.answerdigital.ers.auth.RefreshTokenService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.oauth2.client.OAuth2AuthorizedClient;
import org.springframework.security.oauth2.client.annotation.RegisteredOAuth2AuthorizedClient;
import org.springframework.security.oauth2.core.OAuth2AccessToken;
import org.springframework.security.oauth2.core.OAuth2AuthenticatedPrincipal;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.util.Map;

@Controller
public class MainController {

    @Autowired
    private ERSService service;

    @Autowired
    private RefreshTokenService refreshService;

    @GetMapping("/error")
    public String error(HttpServletResponse response) {
        return "error";
    }

    @GetMapping("/home")
    public String index(Model model) {

        return "index"; //view
    }

    @GetMapping("/success")
    public String authSuccessful(@RegisteredOAuth2AuthorizedClient OAuth2AuthorizedClient client, @AuthenticationPrincipal OAuth2AuthenticatedPrincipal user, Model model) {
        Map<String, Object> claims = user.getAttributes();
        OAuth2AccessToken accessToken = client.getAccessToken();
        String accessTokenString = accessToken.getTokenValue();
        String expiresIn = accessToken.getExpiresAt().toString();

        model.addAttribute("userName", user.getName());
        model.addAttribute("userId", user.getAttribute("sub"));
        model.addAttribute("accessToken", accessTokenString);
        model.addAttribute("expiresIn", expiresIn);

        return "auth_successful"; //view
    }

    @GetMapping("/handover")
    public String sessionHandoverResponse(@RegisteredOAuth2AuthorizedClient OAuth2AuthorizedClient client, @AuthenticationPrincipal OAuth2AuthenticatedPrincipal user, Model model) throws Exception {

        DateTimeFormatter formatter = DateTimeFormatter.ISO_DATE_TIME.withZone(ZoneId.of("UTC"));

        AuthenticatedSession session = new AuthenticatedSession();
        session.setAuthenticationToken(client.getAccessToken().getTokenValue());
        session.setName(user.getName());
        session.setUserId(user.getAttribute("sub"));

        AuthenticatedSessionResponse response = null;
        try {
            response = service.handover(session);
        } catch (IOException e){
            model.addAttribute("success", false);
            model.addAttribute("message", e.getMessage());
        }
        if (response != null) {
            model.addAttribute("success", true);
            model.addAttribute("responseCode", response.getResponseCode());
            model.addAttribute("message", response.getMessage());
        }

        refreshService.put(client, SecurityContextHolder.getContext().getAuthentication());

        return "handover_outcome"; //view
    }
}