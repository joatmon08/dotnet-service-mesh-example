service {
  name    = "expense"
  id      = "expense-v2"
  address = "10.5.0.6"
  port    = 5001

  tags = ["v2"]
  meta = {
    version = "2"
  }

  connect {
    sidecar_service {
      port = 20000

      check {
        name     = "Connect Envoy Sidecar"
        tcp      = "10.5.0.6:20000"
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
                   "max_requests": 1
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
          destination_name   = "expense-db"
          local_bind_address = "127.0.0.1"
          local_bind_port    = 1433
        }
      }
    }
  }
}