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

package org.openmhealth.dsu.controller;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mockito;
import org.openmhealth.dsu.configuration.Application;
import org.openmhealth.dsu.configuration.TestConfiguration;
import org.openmhealth.dsu.domain.EndUserRegistrationData;
import org.openmhealth.dsu.service.EndUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.SpringApplicationConfiguration;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Primary;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit4.SpringJUnit4ClassRunner;
import org.springframework.test.context.web.WebAppConfiguration;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.web.context.WebApplicationContext;

import static org.mockito.Matchers.anyString;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;


/**
 * A suite of integration tests for the end user controller.
 *
 * @author Emerson Farrugia
 */
@RunWith(SpringJUnit4ClassRunner.class)
@SpringApplicationConfiguration(classes = {
        Application.class,
        EndUserControllerIntegrationTests.Configuration.class
})
@WebAppConfiguration
public class EndUserControllerIntegrationTests {

    private static final String REGISTER_USER_URI = "/users";


    // TODO find out if there's a simpler way to define mocks that work with @SpringApplicationConfiguration
    @TestConfiguration
    static class Configuration {

        @Bean
        @Primary
        public EndUserService endUserService() {
            return Mockito.mock(EndUserService.class);
        }
    }


    @Autowired
    private WebApplicationContext applicationContext;

    @Autowired
    private ObjectMapper objectMapper;

    @Autowired
    private EndUserService mockEndUserService;

    private MockMvc mockMvc;
    private EndUserRegistrationData registrationData;

    @Before
    public void initialiseClientMock() {
        mockMvc = MockMvcBuilders.webAppContextSetup(this.applicationContext).build();
    }

    @Before
    public void initialiseFixture() {

        registrationData = new EndUserRegistrationData();
        registrationData.setUsername("test");
        registrationData.setPassword("test");
        registrationData.setEmailAddress("test@test.test");
    }

    @Before
    public void initialiseMocks() {
        when(mockEndUserService.doesUserExist(anyString())).thenReturn(false);
    }

    @After
    public void resetMocks() {
        reset(mockEndUserService);
    }

    @Test
    public void registerUserShouldReturnUnsupportedMediaTypeOnNonJsonContentType() throws Exception {

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_XML)
                        .content("<data />"))
                .andExpect(status().isUnsupportedMediaType());
    }

    @Test
    public void registerUserShouldReturnBadRequestOnEmptyContent() throws Exception {

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{}"))
                .andExpect(status().isBadRequest());
    }

    @Test
    public void registerUserShouldReturnBadRequestOnUndefinedUsername() throws Exception {

        registrationData.setUsername(null);

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isBadRequest());
    }

    @Test
    public void registerUserShouldReturnBadRequestOnEmptyUsername() throws Exception {

        registrationData.setUsername("");

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isBadRequest());
    }

    @Test
    public void registerUserShouldReturnBadRequestOnUndefinedPassword() throws Exception {

        registrationData.setPassword(null);

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isBadRequest());
    }

    @Test
    public void registerUserShouldReturnBadRequestOnEmptyPassword() throws Exception {

        registrationData.setPassword("");

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isBadRequest());
    }

    @Test
    public void registerUserShouldReturnBadRequestOnMalformedEmailAddress() throws Exception {

        registrationData.setEmailAddress("foo");

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isBadRequest());
    }

    @Test
    public void registerUserShouldReturnConflictOnExistingUsername() throws Exception {

        when(mockEndUserService.doesUserExist(registrationData.getUsername())).thenReturn(true);

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isConflict());
    }

    @Test
    public void registerUserShouldReturnCreatedOnContentHavingEmailAddress() throws Exception {

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isCreated());

        verify(mockEndUserService).registerUser(registrationData);
    }

    @Test
    public void registerUserShouldReturnCreatedOnContentNotHavingEmailAddress() throws Exception {

        registrationData.setEmailAddress(null);

        mockMvc.perform(
                post(REGISTER_USER_URI)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(registrationData)))
                .andExpect(status().isCreated());

        verify(mockEndUserService).registerUser(registrationData);
    }
}