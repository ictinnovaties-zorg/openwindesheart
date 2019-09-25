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

package org.openmhealth.dsu.configuration;

/**
 * An interface containing shared properties for OAuth2 configurations.
 *
 * @author Emerson Farrugia
 */
public interface OAuth2Properties {

    String CLIENT_ROLE = "ROLE_CLIENT";
    String END_USER_ROLE = "ROLE_END_USER";
    String DATA_POINT_RESOURCE_ID = "dataPoints";
    String DATA_POINT_READ_SCOPE = "read_data_points";
    String DATA_POINT_WRITE_SCOPE = "write_data_points";
    String DATA_POINT_DELETE_SCOPE = "delete_data_points";
}
