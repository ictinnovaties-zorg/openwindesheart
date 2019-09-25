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

package org.openmhealth.dsu.repository;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.openmhealth.dsu.domain.DataPointSearchCriteria;
import org.openmhealth.schema.domain.omh.DataPoint;
import org.springframework.beans.factory.annotation.Autowired;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static com.google.common.collect.Lists.newArrayList;
import static org.hamcrest.CoreMatchers.equalTo;
import static org.hamcrest.Matchers.empty;
import static org.hamcrest.Matchers.hasSize;
import static org.junit.Assert.assertThat;
import static org.openmhealth.dsu.factory.DataPointFactory.*;


/**
 * A suite of integration tests for a data point repository.
 *
 * @author Emerson Farrugia
 */
public abstract class DataPointRepositoryIntegrationTests {

    public static final String UNRECOGNIZED_ID = "foo";

    @Autowired
    protected DataPointRepository repository;

    private DataPoint testDataPoint;
    private List<DataPoint> testDataPoints;


    @Before
    public void initialiseFixture() {

        testDataPoint = repository.save(newDataPointBuilder().build());

        testDataPoints = new ArrayList<>();
        testDataPoints.add(testDataPoint);
    }

    @After
    public void deleteFixture() {

        for (DataPoint dataPoint : testDataPoints) {
            repository.delete(dataPoint.getHeader().getId());
        }
    }

    @Test
    public void existsShouldReturnFalseOnUnrecognizedId() {

        assertThat(repository.exists(UNRECOGNIZED_ID), equalTo(false));
    }

    @Test
    public void existsShouldReturnTrueOnMatchingId() {

        assertThat(repository.exists(testDataPoint.getHeader().getId()), equalTo(true));
    }

    @Test
    public void findOneShouldReturnNotPresentOnUnrecognizedId() {

        Optional<DataPoint> result = repository.findOne(UNRECOGNIZED_ID);

        assertThat(result.isPresent(), equalTo(false));
    }

    @Test
    public void findOneShouldReturnDataPointMatchingId() {

        Optional<DataPoint> result = repository.findOne(testDataPoint.getHeader().getId());

        assertThat(result.isPresent(), equalTo(true));
        assertThatDataPointsAreEqual(result.get(), testDataPoint);
    }

    public void assertThatDataPointsAreEqual(DataPoint actual, DataPoint expected) {
        assertThat(actual, equalTo(expected));
    }

    @Test(expected = NullPointerException.class)
    public void findBySearchCriteriaShouldThrowExceptionWithUndefinedCriteria() {

        repository.findBySearchCriteria(null, null, null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void findBySearchCriteriaShouldThrowExceptionWithNegativeOffset() {

        repository.findBySearchCriteria(newSearchCriteriaBuilder().build(), -1, null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void findBySearchCriteriaShouldThrowExceptionWithNegativeLimit() {

        repository.findBySearchCriteria(newSearchCriteriaBuilder().build(), null, -1);
    }

    @Test
    // identical to
    // findBySearchCriteriaShouldReturnDataPointsMatchingSchemaMajorVersion
    // findBySearchCriteriaShouldReturnDataPointsMatchingSchemaMinorVersion
    // findBySearchCriteriaShouldReturnDataPointsMatchingSchemaName
    // findBySearchCriteriaShouldReturnDataPointsMatchingSchemaNamespace
    public void findBySearchCriteriaShouldReturnDataPointsMatchingUserId() {

        List<DataPoint> dataPoints =
                newArrayList(repository.findBySearchCriteria(newSearchCriteriaBuilder().build(), null, null));

        assertThat(dataPoints, hasSize(1));
        assertThatDataPointsAreEqual(dataPoints.get(0), testDataPoint);
    }

    @Test
    public void findBySearchCriteriaShouldOnlyReturnDataPointsMatchingUserId() {

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().setUserId(UNRECOGNIZED_ID).build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, empty());
    }

    @Test
    public void findBySearchCriteriaShouldOnlyReturnDataPointsMatchingSchemaNamespace() {

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().setSchemaNamespace(UNRECOGNIZED_ID).build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, empty());
    }

    @Test
    public void findBySearchCriteriaShouldOnlyReturnDataPointsMatchingSchemaName() {

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().setSchemaName(UNRECOGNIZED_ID).build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, empty());
    }

    @Test
    public void findBySearchCriteriaShouldOnlyReturnDataPointsMatchingSchemaMajorVersion() {

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().setSchemaVersionMajor(0).build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, empty());
    }

    @Test
    public void findBySearchCriteriaShouldOnlyReturnDataPointsMatchingSchemaMinorVersion() {

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().setSchemaVersionMinor(0).build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, empty());
    }

    @Test
    public void findBySearchCriteriaShouldReturnDataPointsMatchingSchemaVersionQualifier() {

        DataPoint newTestDataPoint = newDataPointBuilder().setSchemaVersionQualifier("RC1").build();

        newTestDataPoint = repository.save(newTestDataPoint);
        testDataPoints.add(newTestDataPoint);

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().setSchemaVersionQualifier("RC1").build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, hasSize(1));
        assertThatDataPointsAreEqual(dataPoints.get(0), newTestDataPoint);
    }

    @Test
    public void findBySearchCriteriaShouldOnlyReturnDataPointsMatchingSchemaVersionQualifier() {

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().setSchemaVersionQualifier("RC1").build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, empty());
    }

    @Test
    public void findBySearchCriteriaShouldOnlyReturnDataPointsMatchingMissingSchemaVersionQualifier() {

        DataPoint newTestDataPoint = newDataPointBuilder().setSchemaVersionQualifier("RC1").build();

        newTestDataPoint = repository.save(newTestDataPoint);
        testDataPoints.add(newTestDataPoint);

        DataPointSearchCriteria searchCriteria = newSearchCriteriaBuilder().build();

        List<DataPoint> dataPoints = newArrayList(repository.findBySearchCriteria(searchCriteria, null, null));

        assertThat(dataPoints, hasSize(1));
        assertThatDataPointsAreEqual(dataPoints.get(0), testDataPoint);
    }

    @Test
    public void deleteShouldNotThrowExceptionOnUnrecognizedId() {

        repository.delete(UNRECOGNIZED_ID);
    }

    @Test
    public void deleteShouldDeleteDataPointMatchingId() {

        repository.delete(testDataPoint.getHeader().getId());

        assertThat(repository.exists(testDataPoint.getHeader().getId()), equalTo(false));
    }

    @Test
    public void deleteByIdAndUserIdShouldReturnZeroOnUnrecognizedId() {

        assertThat(repository.deleteByIdAndHeaderUserId(UNRECOGNIZED_ID, TEST_USER_ID), equalTo(0l));
    }

    @Test
    public void deleteByIdAndUserIdShouldReturnZeroOnUnrecognizedUserId() {

        assertThat(repository.deleteByIdAndHeaderUserId(testDataPoint.getHeader().getId(), UNRECOGNIZED_ID),
                equalTo(0l));
    }

    @Test
    public void deleteByIdAndUserIdShouldReturnOneOnMatchingIdAndUserId() {

        assertThat(repository.deleteByIdAndHeaderUserId(testDataPoint.getHeader().getId(), TEST_USER_ID), equalTo(1l));
        assertThat(repository.exists(testDataPoint.getHeader().getId()), equalTo(false));
    }
}
