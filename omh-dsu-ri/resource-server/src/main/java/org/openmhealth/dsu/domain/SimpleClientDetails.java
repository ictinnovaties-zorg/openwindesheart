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
import org.springframework.security.oauth2.provider.ClientDetails;
import org.springframework.security.oauth2.provider.client.BaseClientDetails;


/**
 * The details of an OAuth 2.0 client. This class only exists to extend {@link BaseClientDetails} and mark the client
 * identifier as the Spring Data identifier used for persistence.
 *
 * @author Emerson Farrugia
 */
public class SimpleClientDetails extends BaseClientDetails {

    @Id
    private String id;

    public SimpleClientDetails() {
    }

    public SimpleClientDetails(ClientDetails prototype) {
        super(prototype);
        this.id = getClientId();
    }

    public SimpleClientDetails(String clientId, String resourceIds, String scopes, String grantTypes,
            String authorities) {

        super(clientId, resourceIds, scopes, grantTypes, authorities);
        this.id = getClientId();
    }

    public SimpleClientDetails(String clientId, String resourceIds, String scopes, String grantTypes,
            String authorities, String redirectUris) {

        super(clientId, resourceIds, scopes, grantTypes, authorities, redirectUris);
        this.id = getClientId();
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        setClientId(id);
    }

    @Override
    public void setClientId(String clientId) {
        super.setClientId(clientId);
        this.id = clientId;
    }
}
