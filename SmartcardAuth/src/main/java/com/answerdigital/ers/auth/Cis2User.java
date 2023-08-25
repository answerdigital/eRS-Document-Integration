package com.answerdigital.ers.auth;

import com.fasterxml.jackson.annotation.JsonProperty;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.oauth2.core.user.DefaultOAuth2User;
import org.springframework.security.oauth2.core.user.OAuth2User;

import java.util.Collection;
import java.util.List;
import java.util.Map;

public class Cis2User implements OAuth2User {

    private OAuth2User delegate;

    private List<Cis2RbacRole> roles;
    @JsonProperty("selected_roleid")
    public String getSelectedRoleId() {
        return selectedRoleId;
    }
    @JsonProperty("selected_roleid")
    public void setSelectedRoleId(String selectedRoleId) {
        this.selectedRoleId = selectedRoleId;
    }

    private String selectedRoleId;

    @Override
    public Map<String, Object> getAttributes() {
        return null;
    }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        return null;
    }

    @Override
    public String getName() {
        return null;
    }
    @JsonProperty("nhsid_nrbac_roles")
    public List<Cis2RbacRole> getRoles() {
        return roles;
    }
    @JsonProperty("nhsid_nrbac_roles")
    public void setRoles(List<Cis2RbacRole> roles) {
        this.roles = roles;
    }
}
