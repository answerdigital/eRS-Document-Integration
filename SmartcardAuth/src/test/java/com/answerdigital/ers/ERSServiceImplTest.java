package com.answerdigital.ers;

import com.answerdigital.ers.api.AuthenticatedSession;
import com.answerdigital.ers.api.AuthenticatedSessionResponse;
import com.answerdigital.ers.api.ERSService;
import com.answerdigital.ers.api.ERSServiceImpl;
import com.github.tomakehurst.wiremock.junit5.WireMockExtension;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.RegisterExtension;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.DynamicPropertyRegistry;
import org.springframework.test.context.DynamicPropertySource;

import java.io.IOException;

import static com.github.tomakehurst.wiremock.client.WireMock.*;
import static com.github.tomakehurst.wiremock.core.WireMockConfiguration.wireMockConfig;
import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
public class ERSServiceImplTest {

    //todo: setup OAuth 2 mock https://tanzu.vmware.com/content/pivotal-engineering-journal/faking-oauth2-single-sign-on-in-spring-3-ways

    @RegisterExtension
    static WireMockExtension knownCertificateServer = WireMockExtension.newInstance()
            .options(wireMockConfig().dynamicPort().dynamicHttpsPort().keystorePath("known_ca.jks").keystorePassword("changeit"))
            .build();

    @RegisterExtension
    static WireMockExtension unknownCertificateServer = WireMockExtension.newInstance()
            .options(wireMockConfig().dynamicPort().dynamicHttpsPort().keystorePath("unknown_ca.jks").keystorePassword("changeit"))
            .build();

    @DynamicPropertySource
    static void endpointProperties(DynamicPropertyRegistry registry) {
        registry.add("ers.session-handover-endpoint", () ->
                knownCertificateServer.getRuntimeInfo().getHttpsBaseUrl() + "/session"
        );
    }

    //@Test
    public void whenKnownCertificate_thenConnects() throws Exception {

        unknownCertificateServer.stubFor(post("/session").willReturn(
                aResponse()
                        .withBody("{\"code\":\"200\",\"message\":\"OK\"}")
                        .withHeader("Content-Type", "application/json")
                        .withStatus(200)
                )
        );
        String endpoint = unknownCertificateServer.getRuntimeInfo().getHttpsBaseUrl() + "/session";
        ERSService service = new ERSServiceImpl(endpoint);

        AuthenticatedSession session = new AuthenticatedSession();
        session.setUserId("test");
        session.setName("test");
        session.setAuthenticationToken("test");
        AuthenticatedSessionResponse response = service.handover(session);
        assertEquals(200, response.getResponseCode());
    }

    //@Test
    public void whenUnknownCertificate_thenCannotConnect() throws Exception {
        unknownCertificateServer.stubFor(post("/session").willReturn(
                        aResponse()
                                .withBody("{\"code\":\"200\",\"message\":\"OK\"}")
                                .withHeader("Content-Type", "application/json")
                                .withStatus(200)
                )
        );
        String endpoint = unknownCertificateServer.getRuntimeInfo().getHttpsBaseUrl() + "/session";
        ERSService service = new ERSServiceImpl(endpoint);

        AuthenticatedSession session = new AuthenticatedSession();
        session.setUserId("test");
        session.setName("test");
        session.setAuthenticationToken("test");

        Exception exception = assertThrows(IOException.class, () -> {
            service.handover(session);
        });

        String expectedMessage = "certificate";//TODO
        String actualMessage = exception.getMessage();

        assertTrue(actualMessage.contains(expectedMessage));
    }
}
