/*
 * Copyright 2016 Open mHealth
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

package org.springframework.data.mapping.model;

import com.fasterxml.jackson.annotation.JsonProperty;
import org.springframework.data.mapping.PersistentProperty;


/**
 * A composite field naming strategy that uses a Jackson {@link JsonProperty} annotation value if present,
 * or delegates to a backing strategy otherwise.
 *
 * @author Emerson Farrugia
 */
public class JsonPropertyPreservingFieldNamingStrategy implements FieldNamingStrategy {

    private FieldNamingStrategy backingStrategy;

    /**
     * @param backingStrategy the field naming strategy to use if the {@link JsonProperty} annotation isn't present
     */
    public JsonPropertyPreservingFieldNamingStrategy(FieldNamingStrategy backingStrategy) {

        if (backingStrategy == null) {
            throw new NullPointerException("A backing strategy hasn't been specified.");
        }

        this.backingStrategy = backingStrategy;
    }

    @Override
    public String getFieldName(PersistentProperty<?> property) {

        JsonProperty jsonPropertyAnnotation = property.findAnnotation(JsonProperty.class);

        if (jsonPropertyAnnotation != null) {
            return jsonPropertyAnnotation.value();
        }
        else {
            return backingStrategy.getFieldName(property);
        }
    }
}
