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

/**
 * An exception thrown to indicate a problem registering an end user.
 *
 * @author Emerson Farrugia
 */
public class EndUserRegistrationException extends RuntimeException {

    private EndUserRegistrationData registrationData;

    public EndUserRegistrationException(EndUserRegistrationData registrationData) {
        this.registrationData = registrationData;
    }

    public EndUserRegistrationException(EndUserRegistrationData registrationData, Throwable cause) {
        super(cause);
        this.registrationData = registrationData;
    }

    public EndUserRegistrationData getRegistrationData() {
        return registrationData;
    }
}
