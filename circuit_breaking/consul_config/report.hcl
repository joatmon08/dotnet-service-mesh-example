service {
  name = "report"
  address = "10.5.0.5"
  port= 5002
  connect { 
    sidecar_service {
      port = 20000

  		checks {
        name = "Connect Sidecar Listening"
        tcp = "10.5.0.5:20000"
			  interval = "10s"
      }

      proxy {
				config {
          envoy_local_cluster_json = <<EOL
           {
             "@type": "type.googleapis.com/envoy.api.v2.Cluster",
             "name": "local_app",
             "connect_timeout": "5s",
             "circuit_breakers": {
               "thresholds": [
                 {
                   "priority": "HIGH",
                   "max_requests": 10
                 }
               ]
             },
             "load_assignment": {
              "cluster_name": "local_app",
              "endpoints": [
               {
                "lb_endpoints": [
                 {
                  "endpoint": {
                   "address": {
                    "socket_address": {
                     "address": "127.0.0.1",
                     "port_value": 5001
                    }
                   }
                  }
                 }
                ]
               }
              ]
             }
           }
        EOL
        
        }

        upstreams {
          destination_name = "expense"
          local_bind_address = "127.0.0.1"
          local_bind_port = 5001

          config {
            envoy_cluster_json = <<EOL
              {
                "@type": "type.googleapis.com/envoy.api.v2.Cluster",
                "name": "expense",
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
              "name": "expense:127.0.0.1:5001",
              "address": {
                "socketAddress": {
                  "address": "127.0.0.1",
                  "portValue": 5001
                }
              },
              "filterChains": [
                {
                  "filters": [
                    {
                      "name": "envoy.http_connection_manager",
                      "config": {
                        "stat_prefix": "expense",
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
                                    "cluster": "expense",
                                    "timeout": "6s",
                                    "retry_policy": {
                                      "retry_on": "5xx",
                                      "num_retries": 5,
                                      "per_try_timeout": "2s"
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
