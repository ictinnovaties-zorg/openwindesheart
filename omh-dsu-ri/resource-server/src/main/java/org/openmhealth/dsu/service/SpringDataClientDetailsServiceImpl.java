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

package org.openmhealth.dsu.service;

import org.openmhealth.dsu.domain.SimpleClientDetails;
import org.openmhealth.dsu.repository.ClientDetailsRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.oauth2.provider.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.List;

import static java.lang.String.format;


/**
 * An implementation of Spring Security's {@link ClientDetailsService} and {@link ClientRegistrationService} that
 * persists OAuth 2.0 client details using Spring Data repositories.
 *
 * @author Emerson Farrugia
 */
@Service
public class SpringDataClientDetailsServiceImpl implements ClientDetailsService, ClientRegistrationService {

    @Autowired
    private ClientDetailsRepository repository;

    @Override
    @Transactional(readOnly = true)
    public SimpleClientDetails loadClientByClientId(String clientId) throws ClientRegistrationException {

        return repository.findOne(clientId).orElseThrow(() ->
                new NoSuchClientException(format("A client with id '%s' wasn't found.", clientId)));
    }

    @Override
    @Transactional
    public void addClientDetails(ClientDetails clientDetails) throws ClientAlreadyExistsException {

        if (repository.findOne(clientDetails.getClientId()).isPresent()) {
            throw new ClientAlreadyExistsException(
                    format("A client with id '%s' already exists.", clientDetails.getClientId()));
        }

        repository.save(new SimpleClientDetails(clientDetails));
    }

    @Override
    @Transactional
    public void updateClientDetails(ClientDetails clientDetails) throws NoSuchClientException {

        if (!repository.findOne(clientDetails.getClientId()).isPresent()) {
            throw new NoSuchClientException(format("A client with id '%s' wasn't found.", clientDetails.getClientId()));
        }

        repository.save(new SimpleClientDetails(clientDetails));
    }

    @Override
    @Transactional
    public void updateClientSecret(String clientId, String secret) throws NoSuchClientException {

        SimpleClientDetails clientDetails = repository.findOne(clientId).orElseThrow(() ->
                new NoSuchClientException(format("A client with id '%s' wasn't found.", clientId)));

        clientDetails.setClientSecret(secret);

        repository.save(clientDetails);
    }

    @Override
    @Transactional
    public void removeClientDetails(String clientId) throws NoSuchClientException {

        if (!repository.findOne(clientId).isPresent()) {
            throw new NoSuchClientException(format("A client with id '%s' wasn't found.", clientId));
        }

        repository.delete(clientId);
    }

    @Override
    @Transactional(readOnly = true)
    public List<ClientDetails> listClientDetails() {

        List<ClientDetails> list = new ArrayList<>();

        for (ClientDetails clientDetails : repository.findAll()) {
            list.add(clientDetails);
        }

        return list;
    }
}
