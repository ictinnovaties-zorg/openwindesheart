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

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.security.authentication.AuthenticationCredentialsNotFoundException;
import org.springframework.web.bind.MissingServletRequestParameterException;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.ResponseStatus;

import javax.servlet.http.HttpServletRequest;


/**
 * A set of exception handlers that are applied to all controllers, primarily to prevent information disclosure.
 *
 * @author Emerson Farrugia
 */
@ControllerAdvice
public class GenericExceptionHandlingControllerAdvice {

    private static final Logger log = LoggerFactory.getLogger(GenericExceptionHandlingControllerAdvice.class);

    @ExceptionHandler(AuthenticationCredentialsNotFoundException.class)
    @ResponseStatus(HttpStatus.UNAUTHORIZED)
    public void handleAuthenticationCredentialsNotFoundException(Exception e, HttpServletRequest request) {

        log.debug("A {} request for '{}' failed authentication.", request.getMethod(), request.getPathInfo(), e);
    }

    @ExceptionHandler(MissingServletRequestParameterException.class)
    @ResponseStatus(HttpStatus.BAD_REQUEST)
    public void handleMissingServletRequestParameterException(MissingServletRequestParameterException e,
            HttpServletRequest request) {

        log.debug("A {} request for '{}' failed because parameter '{}' is missing.",
                request.getMethod(), request.getPathInfo(), e.getParameterName(), e);
    }

    @ExceptionHandler(Exception.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public void handleException(Exception e, HttpServletRequest request) {

        log.warn("A {} request for '{}' failed.", request.getMethod(), request.getPathInfo(), e);
    }
}