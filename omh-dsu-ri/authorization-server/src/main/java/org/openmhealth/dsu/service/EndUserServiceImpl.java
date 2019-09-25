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

import org.openmhealth.dsu.domain.EndUser;
import org.openmhealth.dsu.domain.EndUserRegistrationData;
import org.openmhealth.dsu.domain.EndUserRegistrationException;
import org.openmhealth.dsu.repository.EndUserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;
import java.time.OffsetDateTime;
import java.util.Optional;


/**
 * @author Emerson Farrugia
 */
@Service
public class EndUserServiceImpl implements EndUserService {

    @Autowired
    private EndUserRepository endUserRepository;

    private PasswordEncoder passwordEncoder = new BCryptPasswordEncoder();

    @Override
    @Transactional(readOnly = true)
    public boolean doesUserExist(String username) {

        return endUserRepository.findOne(username).isPresent();
    }

    @Override
    @Transactional
    public void registerUser(EndUserRegistrationData registrationData) {

        if (doesUserExist(registrationData.getUsername())) {
            throw new EndUserRegistrationException(registrationData);
        }

        EndUser endUser = new EndUser();
        endUser.setUsername(registrationData.getUsername());
        endUser.setPasswordHash(passwordEncoder.encode(registrationData.getPassword()));
        endUser.setRegistrationTimestamp(OffsetDateTime.now());

        if (registrationData.getEmailAddress() != null) {
            try {
                endUser.setEmailAddress(new InternetAddress(registrationData.getEmailAddress()));
            }
            catch (AddressException e) {
                throw new EndUserRegistrationException(registrationData, e);
            }
        }

        endUserRepository.save(endUser);
    }

    @Override
    @Transactional(readOnly = true)
    public Optional<EndUser> findUser(String username) {

        return endUserRepository.findOne(username);
    }
}