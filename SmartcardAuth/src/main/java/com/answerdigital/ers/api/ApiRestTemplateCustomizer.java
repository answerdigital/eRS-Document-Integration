package com.answerdigital.ers.api;

import java.io.ByteArrayInputStream;
import java.net.URL;
import java.security.KeyStore;
import java.security.cert.Certificate;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import javax.net.ssl.SSLContext;

import org.apache.http.client.HttpClient;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.ssl.SSLContextBuilder;
import org.apache.http.ssl.SSLContexts;
import org.springframework.boot.web.client.RestTemplateCustomizer;
import org.springframework.http.client.ClientHttpRequestFactory;
import org.springframework.http.client.HttpComponentsClientHttpRequestFactory;
import org.springframework.stereotype.Component;
import org.springframework.web.client.RestTemplate;

@Component
public class ApiRestTemplateCustomizer implements RestTemplateCustomizer {

    public static final String DEFAULT_CERT = "-----BEGIN CERTIFICATE-----\n" +
            "MIIFCTCCAvGgAwIBAgIUNy5yj5AlyC+uRa34jJJaDp2+YdswDQYJKoZIhvcNAQEL\n" +
            "BQAwFDESMBAGA1UEAwwJbG9jYWxob3N0MB4XDTIzMDMyMjIxMTUxOFoXDTI0MDMy\n" +
            "MTIxMTUxOFowFDESMBAGA1UEAwwJbG9jYWxob3N0MIICIjANBgkqhkiG9w0BAQEF\n" +
            "AAOCAg8AMIICCgKCAgEApwR1Pk4h4SFLJXjETc2pFRoKCTXYyV6HQpHVgaSOYR97\n" +
            "i86GeCRp+3+DC9aLVP7ZBVlC6Kj3xoELjak/HSAgAzkt3l5oIEzVoJRWOBw46wkX\n" +
            "wrKs4xW2CupQLFjG7kVY6vIOtUYTtJQ2RGv1lTEesMG5bJjQx0TXN843DAJWJxaP\n" +
            "/mF6iALGx/1yfmFvvwPPHtY2nmre74E42IdPztpSUXaUTo+gPhjV1DG1P1ZYBpum\n" +
            "Anehc0475ihfUGBOYCesSMsuyT+1t1jr3ncAe8oGw4Soj6VECAcz6uEi/Q//h9BH\n" +
            "G1aw8PpwIZn9JILhyeWVnDyFRUfuD/xGiPT0hatM4h4C8P7P94rDCvVaH0kKFgVs\n" +
            "rsX6WgeyiTDeCMQ0pTgEBMGf7Yr3923W6tPB4qPJdms0FxN4l7bpvTIHWzOcMwxa\n" +
            "iEr8/pQQSLnMK0FEkq/tIq7f2ywmCGy8sSlqDwN0dcPSG1U+7Pc1UhLLSe3B2xqI\n" +
            "0U+ALa/MPXfw8vVVERgbzEQPJVlGyhY9H+ytPseNJ5em2W1KABfC6uuuuZgpuRv/\n" +
            "mYec5FWDLqwC0D0uC0pknfEntXTI6iIwC8zKYSMvXVZETN1szNrDj9ZLIdowkjhi\n" +
            "NGR4zJp2ZTQPsDiDMo4EV+2e1RlGnl6VLhq8HvUi3mqsBqn2F7T+5c6g5BhWcAkC\n" +
            "AwEAAaNTMFEwHQYDVR0OBBYEFE8a63ccTFneSoKk1kX3WXriNtF4MB8GA1UdIwQY\n" +
            "MBaAFE8a63ccTFneSoKk1kX3WXriNtF4MA8GA1UdEwEB/wQFMAMBAf8wDQYJKoZI\n" +
            "hvcNAQELBQADggIBAB7ggH7k230TDevu7PyLUJD+fs2vvjBAOMvMEZJbnU3NyWt0\n" +
            "6scQXQ4lP7sz3AI+QfBDb50WeYxTTv63II/oimkzT0lYky1iLCYVwZBsgK8jbtZb\n" +
            "OocRpDyPEix5TrBMhrJ2o3S9ir4/sjRMvO1vRTl3uMxa/miDKwMQW7K9mkr2CdEy\n" +
            "6/JawJH/KgVQLIOI+Q7zEDP/nyn7COvsjlo5reLg4gncQvrs+B/43YRm/+PMHkkA\n" +
            "/sI7OX2/8NIBQ4k5wOMaNBJqGg7Jd05rjRLcz2diQD3CdPJaFFixsZdVCswTnvGa\n" +
            "WPpF0cwTre94bIknVgQUGABgjYO7Il+ZjHmazgJZEfQV6oLPavxHB7zE16gGz131\n" +
            "SDaYakjHHjyENB1ELnJVwLtB3nUEgZ/CuUbdCp/ZvvS2WmuFvzpE/PC8BjFdZEh1\n" +
            "aoAcWgGq7e+cH5fp2fe+gWl7fAzojAWYaOKOPr7JvREZtV9Wvw0u8oWflYXYT3Hb\n" +
            "QTEO+FB76L7XbFUFoHuyGwI89kXwGg+SwjEVjUxmfrrq+CJYWfyiBiWRrpJ/6GZO\n" +
            "ffpljj7zUhR7ZWd4RFJUiflB5CIrl2B2kiuee39jjs/Oa1AmxyYdMP4OIbWfsgUI\n" +
            "6HHBy2cR9O4KrBEVJjF0B//KM25MDH6IfYa3hq0OLU/qeINHlDhXTlQF3XA0\n" +
            "-----END CERTIFICATE-----\n";
    @Override
    public void customize(RestTemplate restTemplate) {

        final SSLContext sslContext;
        try {
            sslContext = this.createSSLContext(DEFAULT_CERT);
        } catch (Exception e) {
            throw new IllegalStateException(
                    "Failed to setup client SSL context", e
            );
        }

        final HttpClient httpClient = HttpClientBuilder.create()
                .setSSLContext(sslContext)
                .build();

        final ClientHttpRequestFactory requestFactory =
                new HttpComponentsClientHttpRequestFactory(httpClient);

        restTemplate.setRequestFactory(requestFactory);
    }

    private SSLContext createSSLContext(String certChain) throws Exception {
        CertificateFactory cf = CertificateFactory.getInstance("X.509");
        Certificate cert1 = cf.generateCertificate(new ByteArrayInputStream(certChain.getBytes()));
        KeyStore keyStore = KeyStore.getInstance("pkcs12");
        keyStore.load(null, null);
// TODO: add multiple truststore certificates here
        keyStore.setCertificateEntry("custom-ca-1", cert1);
        return SSLContexts.custom().setKeyStoreType("pkcs12")
                .loadTrustMaterial(keyStore, null)
                .build();
    }
}
