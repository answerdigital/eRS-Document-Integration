package com.answerdigital.ers.api;

import java.io.IOException;

public interface ERSService {
    public AuthenticatedSessionResponse handover(AuthenticatedSession authenticatedSession) throws IOException;
}
