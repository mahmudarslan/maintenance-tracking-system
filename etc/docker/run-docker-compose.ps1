docker network create arslan.vms-network
docker-compose  --progress=plain -f docker-compose.yml -f docker-compose.infrastructure.yml -f docker-compose.infrastructure.override.yml up -d