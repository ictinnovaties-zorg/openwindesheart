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
import java.util.Optional;

import static com.google.common.base.Preconditions.checkArgument;
import static com.google.common.base.Preconditions.checkNotNull;
import static com.google.common.base.Strings.isNullOrEmpty;
import static org.openmhealth.schema.domain.omh.SchemaId.isValidName;
import static org.openmhealth.schema.domain.omh.SchemaId.isValidNamespace;
import static org.openmhealth.schema.domain.omh.SchemaVersion.isValidVersion;


/**
 * A search criteria bean used to represent a search for data points.
 *
 * @author Emerson Farrugia
 */
public class DataPointSearchCriteria {

    private String userId;
    private String schemaNamespace;
    private String schemaName;
    private SchemaVersion schemaVersion;
    private Range<OffsetDateTime> creationTimestampRange;

    public DataPointSearchCriteria(String userId, String schemaNamespace, String schemaName, String schemaVersion) {

        checkNotNull(userId);
        checkArgument(!isNullOrEmpty(userId));

        // TODO determine how restrictive the search criteria should be
        checkNotNull(schemaNamespace);
        checkNotNull(schemaName);
        checkNotNull(schemaVersion);

        checkArgument(isValidNamespace(schemaNamespace));
        checkArgument(isValidName(schemaName));
        checkArgument(isValidVersion(schemaVersion));

        this.userId = userId;
        this.schemaNamespace = schemaNamespace;
        this.schemaName = schemaName;
        this.schemaVersion = new SchemaVersion(schemaVersion);
    }

    public String getUserId() {
        return userId;
    }

    public String getSchemaNamespace() {
        return schemaNamespace;
    }

    public String getSchemaName() {
        return schemaName;
    }

    public SchemaVersion getSchemaVersion() {
        return schemaVersion;
    }

    public Optional<Range<OffsetDateTime>> getCreationTimestampRange() {
        return Optional.ofNullable(creationTimestampRange);
    }

    public void setCreationTimestampRange(Range<OffsetDateTime> creationTimestampRange) {
        this.creationTimestampRange = creationTimestampRange;
    }
}
