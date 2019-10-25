kind = "proxy-defaults"
name = "global"

config {
  envoy_prometheus_bind_addr = "0.0.0.0:9102"

  envoy_extra_static_clusters_json = <<EOL
    {
      "connect_timeout": "1.000s",
      "lb_policy": "ROUND_ROBIN",
      "load_assignment": {
          "cluster_name": "zipkin",
          "endpoints": [
              {
                  "lb_endpoints": [
                      {
                          "endpoint": {
                              "address": {
                                  "socket_address": {
                                      "address": "zipkin",
                                      "port_value": 9411
                                  }
                              }
                          }
                      }
                  ]
              }
          ]
      },
      "name": "zipkin",
      "type": "STRICT_DNS"
    }
  EOL

  envoy_tracing_json = <<EOL
    {
        "http": {
            "config": {
                "collector_cluster": "zipkin",
                "collector_endpoint": "/api/v1/spans"
            },
            "name": "envoy.zipkin"
        }
    }
  EOL
}
