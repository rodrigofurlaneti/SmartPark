# SmartPark — Frontend React

Interface web para o sistema de gestão de estacionamentos **SmartPark**, consumindo a API ASP.NET Core do back-end.

## 🚀 Como rodar

```bash
# 1. Instalar dependências
npm install

# 2. Configurar a URL da API (opcional — padrão: http://localhost:5000/api)
# Edite o arquivo .env
REACT_APP_API_URL=http://localhost:5000/api

# 3. Iniciar o front-end
npm start
```

A aplicação será aberta em [http://localhost:3000](http://localhost:3000).

> **Pré-requisito:** o back-end SmartPark deve estar rodando em `http://localhost:5000`.

---

## 📁 Estrutura do projeto

```
src/
├── components/
│   ├── layout/
│   │   ├── Sidebar.js          # Menu lateral
│   │   └── MainLayout.js       # Layout com sidebar + toast context
│   └── ui/
│       └── index.js            # Componentes reutilizáveis (Button, Card, Table, Modal…)
├── hooks/
│   ├── useApi.js               # Hook genérico para chamadas HTTP
│   ├── useAuth.js              # Hook de autenticação
│   └── useToast.js             # Hook de notificações toast
├── pages/
│   ├── LoginPage.js            # Tela de login
│   ├── DashboardPage.js        # Visão geral / KPIs
│   ├── MovimentacaoPage.js     # Entrada e saída de veículos
│   ├── FaturamentoPage.js      # Controle de turnos e caixa
│   ├── ClientesPage.js         # Cadastro de clientes
│   ├── UnidadesPage.js         # Gestão de unidades/estacionamentos
│   ├── FuncionariosPage.js     # Gestão de funcionários
│   ├── ContasPagarPage.js      # Contas a pagar
│   └── ControlePontoPage.js    # Controle de ponto
├── services/
│   ├── api.js                  # Instância axios com interceptors JWT
│   ├── usuarioService.js       # POST /usuario/login, etc.
│   ├── unidadeService.js       # GET/POST/PUT/DELETE /unidade
│   ├── movimentacaoService.js  # POST /movimentacao/entrada|saida
│   ├── faturamentoService.js   # POST /faturamento/abrir-turno, etc.
│   ├── clienteService.js       # CRUD /cliente
│   ├── funcionarioService.js   # CRUD /funcionario
│   ├── contasAPagarService.js  # CRUD /contasapagar
│   ├── controlePontoService.js # /controleponto
│   └── pedidoSeloService.js    # /pedidoselo
└── utils/
    ├── formatters.js           # formatCurrency, formatDate, formatDuration…
    └── constants.js            # Enums: ESCALAS, FORMAS_PAGAMENTO, etc.
```

---

## 🔌 Endpoints consumidos

| Módulo         | Endpoints                                             |
|----------------|-------------------------------------------------------|
| Autenticação   | `POST /usuario/login`                                 |
| Movimentação   | `POST /movimentacao/entrada`, `POST /movimentacao/saida`, `GET /movimentacao/abertas/{id}` |
| Faturamento    | `POST /faturamento/abrir-turno`, `PUT /faturamento/fechar-turno`, `GET /faturamento/periodo/{id}` |
| Clientes       | `GET/POST /cliente`, `DELETE /cliente/{id}`           |
| Unidades       | `GET/POST /unidade`, `DELETE /unidade/{id}`           |
| Funcionários   | `GET/POST /funcionario`, `DELETE /funcionario/{id}`, `PUT /funcionario/{id}/salario/{valor}` |
| Contas a Pagar | `GET /contasapagar/em-aberto`, `POST /contasapagar`, `PUT /contasapagar/{id}/pagar` |
| Controle Ponto | `GET /controleponto/funcionario/{id}`, `POST /controleponto` |

---

## 🛠 Tecnologias

- **React 18** + hooks
- **React Router DOM 6**
- **Axios** (com interceptor JWT automático)
- **CSS Variables** (tema escuro consistente)
- **Space Grotesk** + **JetBrains Mono** (Google Fonts)

---

## ⚙️ Variáveis de ambiente

| Variável              | Descrição                    | Padrão                       |
|-----------------------|------------------------------|------------------------------|
| `REACT_APP_API_URL`   | Base URL da API back-end     | `http://localhost:5000/api`  |
