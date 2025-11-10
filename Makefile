# ----------------------------
# 🧩 Infra (Postgres + RabbitMQ)
# ----------------------------
up-infra:
	docker compose -f docker-compose.infra.yml up -d

down-infra:
	docker compose -f docker-compose.infra.yml down

# ----------------------------
# ⚙️ Back-end (.NET API + Worker)
# ----------------------------
up-back:
	docker compose -f docker-compose.backend.yml up --build -d

down-back:
	docker compose -f docker-compose.backend.yml down

logs-api:
	docker compose -f docker-compose.backend.yml logs -f api

logs-worker:
	docker compose -f docker-compose.backend.yml logs -f worker

# ----------------------------
# 💻 Front-end (Next.js)
# ----------------------------
up-front:
	docker compose -f docker-compose.frontend.yml up --build -d

down-front:
	docker compose -f docker-compose.frontend.yml down

logs-front:
	docker compose -f docker-compose.frontend.yml logs -f frontend
