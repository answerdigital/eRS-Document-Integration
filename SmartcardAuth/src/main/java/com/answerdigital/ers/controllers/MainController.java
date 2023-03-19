package com.answerdigital.ers.controllers;

import com.answerdigital.ers.api.ERSRequests;

import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.oauth2.client.OAuth2AuthorizedClient;
import org.springframework.security.oauth2.client.annotation.RegisteredOAuth2AuthorizedClient;
import org.springframework.security.oauth2.core.OAuth2AccessToken;
import org.springframework.security.oauth2.core.OAuth2AuthenticatedPrincipal;
import org.springframework.security.oauth2.core.user.OAuth2User;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;

import javax.servlet.http.HttpServletResponse;
import java.util.Map;

@Controller
public class MainController {
    private final String ENDPOINT = System.getenv("ENDPOINT");

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
        model.addAttribute("accessToken", accessTokenString);
        model.addAttribute("expiresIn", expiresIn);

        return "auth_successful"; //view
    }

    @GetMapping("/hello")
    public String helloWorldResponse(@RequestParam("accessToken") String accessToken, Model model) throws Exception {
        model.addAttribute("accessToken", accessToken);

        String[] helloWorldResponse = ERSRequests.makeUserRestrictedRequest(ENDPOINT, accessToken);

        String url = helloWorldResponse[0];
        String responseCode = helloWorldResponse[1];
        String responseBody = helloWorldResponse[2];

        model.addAttribute("url", url);
        model.addAttribute("responseCode", responseCode);
        model.addAttribute("responseBody", responseBody);

        return "hello_world"; //view
    }
}
