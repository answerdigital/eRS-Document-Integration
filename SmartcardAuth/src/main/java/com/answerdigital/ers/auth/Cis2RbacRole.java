package com.answerdigital.ers.auth;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.List;

public class Cis2RbacRole {

    private String ods;
    private String personOrgId;
    private String personRoleId;
    private String orgCode;
    private String roleName;
    private String roleCode;
    private List<String> activities;
    private List<String> activityCodes;

    @JsonProperty("person_orgid")
    public String getPersonOrgId() {
        return personOrgId;
    }
    @JsonProperty("person_orgid")
    public void setPersonOrgId(String personOrgId) {
        this.personOrgId = personOrgId;
    }
    @JsonProperty("person_roleid")
    public String getPersonRoleId() {
        return personRoleId;
    }
    @JsonProperty("person_roleid")
    public void setPersonRoleId(String personRoleId) {
        this.personRoleId = personRoleId;
    }
    @JsonProperty("org_code")
    public String getOrgCode() {
        return orgCode;
    }
    @JsonProperty("org_code")
    public void setOrgCode(String orgCode) {
        this.orgCode = orgCode;
    }
    @JsonProperty("role_name")
    public String getRoleName() {
        return roleName;
    }
    @JsonProperty("role_name")
    public void setRoleName(String roleName) {
        this.roleName = roleName;
    }
    @JsonProperty("role_code")
    public String getRoleCode() {
        return roleCode;
    }
    @JsonProperty("role_code")
    public void setRoleCode(String roleCode) {
        this.roleCode = roleCode;
    }

    public List<String> getActivities() {
        return activities;
    }

    public void setActivities(List<String> activities) {
        this.activities = activities;
    }
    @JsonProperty("activity_codes")
    public List<String> getActivityCodes() {
        return activityCodes;
    }
    @JsonProperty("activity_codes")
    public void setActivityCodes(List<String> activityCodes) {
        this.activityCodes = activityCodes;
    }
}
