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

import com.google.common.collect.Range;
import org.openmhealth.schema.domain.omh.SchemaVersion;

import java.time.OffsetDateTime;


/**
 * A builder that simplifies the construction of data point search criteria.
 *
 * @author Emerson Farrugia
 */
public class DataPointSearchCriteriaBuilder {

    private String userId;
    private String schemaNamespace;
    private String schemaName;
    private Integer schemaVersionMajor;
    private Integer schemaVersionMinor;
    private String schemaVersionQualifier;
    private Range<OffsetDateTime> creationTimestampLowerEndpoint = Range.all();
    private Range<OffsetDateTime> creationTimestampUpperEndpoint = Range.all();

    public DataPointSearchCriteriaBuilder() {

    }

    public DataPointSearchCriteriaBuilder setUserId(String userId) {
        this.userId = userId;
        return this;
    }

    public DataPointSearchCriteriaBuilder setSchemaNamespace(String schemaNamespace) {
        this.schemaNamespace = schemaNamespace;
        return this;
    }

    public DataPointSearchCriteriaBuilder setSchemaName(String schemaName) {
        this.schemaName = schemaName;
        return this;
    }

    public DataPointSearchCriteriaBuilder setSchemaVersionMajor(Integer schemaVersionMajor) {
        this.schemaVersionMajor = schemaVersionMajor;
        return this;
    }

    public DataPointSearchCriteriaBuilder setSchemaVersionMinor(Integer schemaVersionMinor) {
        this.schemaVersionMinor = schemaVersionMinor;
        return this;
    }

    public DataPointSearchCriteriaBuilder setSchemaVersionQualifier(String schemaVersionQualifier) {
        this.schemaVersionQualifier = schemaVersionQualifier;
        return this;
    }

    public DataPointSearchCriteriaBuilder setSchemaVersion(SchemaVersion schemaVersion) {

        if (schemaVersion == null) {
            setSchemaVersionMajor(null);
            setSchemaVersionMinor(null);
            setSchemaVersionQualifier(null);
        }
        else {
            setSchemaVersionMajor(schemaVersion.getMajor());
            setSchemaVersionMinor(schemaVersion.getMinor());
            setSchemaVersionQualifier(schemaVersion.getQualifier().orElse(null));
        }

        return this;
    }

    public DataPointSearchCriteriaBuilder setCreationTimestampLowerEndpoint(OffsetDateTime lowerEndpoint) {
        this.creationTimestampLowerEndpoint = lowerEndpoint != null ? Range.atLeast(lowerEndpoint) : Range.all();
        return this;
    }

    public DataPointSearchCriteriaBuilder setCreationTimestampUpperEndpoint(OffsetDateTime upperEndpoint) {
        this.creationTimestampUpperEndpoint = upperEndpoint != null ? Range.atMost(upperEndpoint) : Range.all();
        return this;
    }

    public DataPointSearchCriteria build() {

        // FIXME this will cause NPEs on unspecified major and minor numbers
        SchemaVersion schemaVersion = new SchemaVersion(schemaVersionMajor, schemaVersionMinor, schemaVersionQualifier);

        DataPointSearchCriteria searchCriteria =
                new DataPointSearchCriteria(userId, schemaNamespace, schemaName, schemaVersion.toString());

        searchCriteria
                .setCreationTimestampRange(creationTimestampLowerEndpoint.intersection(creationTimestampUpperEndpoint));

        return searchCriteria;
    }
}
