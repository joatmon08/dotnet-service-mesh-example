service {
  name    = "expense"
  id      = "expense"
  address = "10.5.0.4"
  port    = 5001

  connect {
    sidecar_service {
      port = 20000

      check {
        name     = "Connect Envoy Sidecar"
        tcp      = "10.5.0.4:20000"
        interval = "10s"
      }

      proxy {
        upstreams {
          destination_name   = "database"
          local_bind_port    = 1433
          config {
            envoy_cluster_json = <<EOL
              {
                "@type": "type.googleapis.com/envoy.api.v2.Cluster",
                "name": "database",
                "type": "EDS",
                "eds_cluster_config": {
                  "eds_config": {
                    "ads": {}
                  }
                },
                "connect_timeout": "5s",
                "outlier_detection": {
                  "consecutive_5xx": 10,
                  "consecutive_gateway_failure": 10,
                  "base_ejection_time": "30s"
                }
              }
            EOL

            envoy_listener_json = <<EOL
              {
              "@type": "type.googleapis.com/envoy.api.v2.Listener",
              "name": "database:127.0.0.1:1433",
              "address": {
                "socketAddress": {
                  "address": "127.0.0.1",
                  "portValue": 1433
                }
              },
              "filterChains": [
                {
                  "filters": [
                    {
                      "name": "envoy.http_connection_manager",
                      "config": {
                        "stat_prefix": "database",
                        "route_config": {
                          "name": "local_route",
                          "virtual_hosts": [
                            {
                              "name": "backend",
                              "domains": ["*"],
                              "routes": [
                                {
                                  "match": {
                                    "prefix": "/"
                                  },
                                  "route": {
                                    "cluster": "database",
                                    "timeout": "6s",
                                    "retry_policy": {
                                      "retry_on": "5xx",
                                      "num_retries": 5
                                    }
                                  }
                                }
                              ]
                            }
                          ]
                        },
                        "http_filters": [
                          {
                            "name": "envoy.router",
                            "config": {}
                          }
                        ]
                      }
                    }
                  ]
                }
              ]
            }
            EOL
          }
        }
      }
    }
  }
}