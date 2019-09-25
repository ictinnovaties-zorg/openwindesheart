/*
 * Copyright 2014 Open mHealth
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package org.openmhealth.dsu.domain;

import org.springframework.data.annotation.Id;

import javax.mail.internet.InternetAddress;
import java.time.OffsetDateTime;
import java.util.Optional;


/**
 * A user account.
 *
 * @author Emerson Farrugia
 */
public class EndUser {

    private String username;
    private String passwordHash;
    private InternetAddress emailAddress;
    private OffsetDateTime registrationTimestamp;

    @Id
    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    /**
     * @return a hash of the user's password
     */
    public String getPasswordHash() {
        return passwordHash;
    }

    public void setPasswordHash(String passwordHash) {
        this.passwordHash = passwordHash;
    }

    public Optional<InternetAddress> getEmailAddress() {
        return Optional.ofNullable(emailAddress);
    }

    public void setEmailAddress(InternetAddress emailAddress) {
        this.emailAddress = emailAddress;
    }

    public OffsetDateTime getRegistrationTimestamp() {
        return registrationTimestamp;
    }

    public void setRegistrationTimestamp(OffsetDateTime registrationTimestamp) {
        this.registrationTimestamp = registrationTimestamp;
    }

    @Override
    @SuppressWarnings("RedundantIfStatement")
    public boolean equals(Object object) {
        if (this == object) {
            return true;
        }
        if (object == null || getClass() != object.getClass()) {
            return false;
        }

        EndUser that = (EndUser) object;

        if (emailAddress != null ? !emailAddress.equals(that.emailAddress) : that.emailAddress != null) {
            return false;
        }
        if (!passwordHash.equals(that.passwordHash)) {
            return false;
        }
        if (!registrationTimestamp.equals(that.registrationTimestamp)) {
            return false;
        }
        if (!username.equals(that.username)) {
            return false;
        }

        return true;
    }

    @Override
    public int hashCode() {
        return username.hashCode();
    }
}
