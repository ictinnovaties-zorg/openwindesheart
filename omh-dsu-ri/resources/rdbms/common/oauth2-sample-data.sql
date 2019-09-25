INSERT INTO oauth_client_details (
  client_id,
  client_secret,
  scope,
  resource_ids,
  authorized_grant_types,
  authorities
)
VALUES (
  'testClient',
  'testClientSecret',
  'read_data_points,write_data_points,delete_data_points',
  'dataPoints',
  'authorization_code,implicit,password,refresh_token',
  'ROLE_CLIENT'
);