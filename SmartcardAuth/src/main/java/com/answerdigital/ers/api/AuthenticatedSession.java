package com.answerdigital.ers.api;

import com.fasterxml.jackson.annotation.JsonProperty;

public class AuthenticatedSession {
    private String authenticationToken;
    private String userId;
    private String name;

    @JsonProperty("nhsid_useruid")
    public String getUserId() {
        return userId;
    }

    @JsonProperty("nhsid_useruid")
    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
    @JsonProperty("auth_token")
    public String getAuthenticationToken() {
        return authenticationToken;
    }
    @JsonProperty("auth_token")
    public void setAuthenticationToken(String authenticationToken) {
        this.authenticationToken = authenticationToken;
    }

}
