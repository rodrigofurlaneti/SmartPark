# Documento de Análise de Requisitos
## Sistema de Gestão de Estacionamentos

**Versão:** 1.0  
**Status:** Em análise  
**Tipo:** Engenharia e especificação de requisitos  
**Objetivo:** Servir como fonte de referência para definição do domínio, regras de negócio, processos, arquitetura e posterior modelagem do banco de dados.

---

# 1. Objetivo do Documento

Este documento tem como objetivo consolidar a análise de requisitos para o desenvolvimento de um sistema de gestão de estacionamentos.

A análise parte de uma abordagem de **engenharia de domínio**, utilizando como referência funcionalidades publicamente apresentadas por sistemas existentes no mercado, disponível no mercado.

O objetivo não é reproduzir internamente o sistema existente, mas abstrair os **processos, conceitos e regras de negócio** necessários para construir uma solução própria.

As informações obtidas a partir de materiais públicos serão classificadas conforme seu nível de certeza:

- **Confirmada:** funcionalidade ou comportamento explicitamente apresentado na documentação pública.
- **Inferida:** comportamento deduzido a partir de uma funcionalidade apresentada.
- **Proposta:** decisão de domínio ou arquitetura definida para este projeto.
- **A validar:** comportamento que deverá ser confirmado posteriormente através do sistema existente, documentação técnica, entrevistas ou testes.

---

# 2. Princípios da Análise

O desenvolvimento deverá seguir a seguinte sequência:

```text
Problema
   ↓
Processo de negócio
   ↓
Regra de negócio
   ↓
Domínio
   ↓
Modelo conceitual
   ↓
Modelo de dados
   ↓
Arquitetura
   ↓
Implementação
   ↓
Integração
```

Não deverá ser adotada a abordagem:

```text
Tela
   ↓
Tabela
   ↓
CRUD
   ↓
API
```

O banco de dados será consequência do domínio e das regras de negócio, e não o contrário.

---

# 3. Escopo do Sistema

O sistema deverá permitir a gestão completa de uma operação de estacionamento, contemplando:

- Empresas;
- Estabelecimentos;
- Matriz e filiais;
- Estacionamentos;
- Usuários;
- Perfis e permissões;
- Clientes;
- Pessoas físicas e jurídicas;
- Veículos;
- Operações de estacionamento;
- Entrada de veículos;
- Tickets;
- Permanência;
- Saída de veículos;
- Tarifação;
- Tabelas de preços;
- Rotativo;
- Mensalistas;
- Convênios;
- Pós-pago;
- Descontos;
- Promoções;
- Tolerância;
- Pagamentos;
- Caixa;
- Faturamento;
- Contas a receber;
- Inadimplência;
- Nota fiscal;
- Funcionários;
- Comissionamento;
- Estoque;
- Serviços adicionais;
- Lava-rápido;
- Agendamento;
- Relatórios;
- Auditoria;
- Integrações externas.

---

# 4. Conceito de Multi-Tenant

O sistema será concebido como uma plataforma **multiempresa / multi-tenant**.

Cada empresa possuirá seus próprios dados e deverá existir isolamento lógico entre os tenants.

Conceitualmente:

```text
PLATAFORMA
│
├── EMPRESA A
│   ├── ESTABELECIMENTO
│   ├── ESTABELECIMENTO
│   └── USUÁRIOS
│
├── EMPRESA B
│   ├── ESTABELECIMENTO
│   └── USUÁRIOS
│
└── EMPRESA C
    ├── ESTABELECIMENTO
    ├── ESTABELECIMENTO
    └── USUÁRIOS
```

## RN-001 — Isolamento entre empresas

Dados pertencentes a uma empresa não poderão ser visualizados, alterados ou utilizados por outra empresa.

**Status:** Proposta.

---

# 5. Modelo Organizacional

Um ponto fundamental do domínio é separar:

- Empresa;
- Estabelecimento;
- Estacionamento.

Esses conceitos não devem ser tratados como a mesma entidade.

## 5.1 Empresa

Representa a organização que utiliza a plataforma.

Exemplo:

```text
ABC Estacionamentos Ltda.
```

Uma empresa pode possuir um ou vários estabelecimentos.

Uma empresa poderá possuir:

- Razão social;
- Nome fantasia;
- CNPJ;
- Dados de contato;
- Configurações;
- Usuários;
- Plano;
- Estabelecimentos;
- Configurações financeiras;
- Configurações operacionais;
- Configurações fiscais;
- Integrações.

---

# 6. Estabelecimento

O estabelecimento representa uma unidade jurídica/fiscal da empresa.

Um estabelecimento poderá ser:

- Matriz;
- Filial.

Exemplo:

```text
ABC Estacionamentos Ltda.
│
├── Matriz
│   └── CNPJ XX.XXX.XXX/0001-XX
│
├── Filial
│   └── CNPJ XX.XXX.XXX/0002-XX
│
└── Filial
    └── CNPJ XX.XXX.XXX/0003-XX
```

## RN-002 — Empresa possui estabelecimentos

Uma empresa poderá possuir um ou mais estabelecimentos.

**Status:** Proposta.

## RN-003 — Estabelecimento possui identificação própria

Cada estabelecimento deverá possuir identificação fiscal própria quando aplicável.

**Status:** Proposta / A validar.

## RN-004 — Matriz e filial pertencem à mesma empresa

Matriz e filiais pertencem ao mesmo tenant, porém podem possuir configurações fiscais e operacionais próprias.

**Status:** Proposta.

---

# 7. Estacionamento

O estacionamento representa a unidade operacional onde ocorre a atividade.

Um estabelecimento poderá possuir um ou mais estacionamentos.

Exemplo:

```text
Empresa
│
└── Filial Guarulhos
    │
    ├── Estacionamento Centro
    │
    ├── Estacionamento Shopping
    │
    └── Estacionamento Aeroporto
```

## RN-005 — Estacionamento pertence a um estabelecimento

Cada estacionamento deverá estar associado a um estabelecimento.

**Status:** Proposta.

## RN-006 — Estacionamento possui configuração operacional própria

Cada estacionamento poderá possuir:

- Endereço;
- Horário de funcionamento;
- Capacidade;
- Vagas;
- Tabelas de preço;
- Operadores;
- Caixas;
- Terminais;
- Métodos de pagamento;
- Configurações de entrada;
- Configurações de saída;
- Convênios;
- Mensalistas.

**Status:** Proposta / A validar.

---

# 8. Estrutura Organizacional

O modelo conceitual deverá ser:

```text
PLATAFORMA
    │
    └── EMPRESA
          │
          ├── ESTABELECIMENTO
          │      ├── MATRIZ
          │      └── FILIAL
          │
          └── ESTACIONAMENTO
```

Com a seguinte relação:

```text
Empresa
   1
   │
   N
Estabelecimento
   1
   │
   N
Estacionamento
```

---

# 9. Usuários

O sistema deverá possuir usuários responsáveis pela operação e administração.

Perfis sugeridos:

- Administrador da plataforma;
- Administrador da empresa;
- Gestor;
- Supervisor;
- Operador;
- Caixa;
- Financeiro;
- Fiscal;
- Funcionário.

Um usuário poderá possuir permissões diferentes conforme sua atuação.

---

# 10. Controle de Acesso

O acesso deverá considerar:

```text
Usuário
   ↓
Perfil
   ↓
Permissões
   ↓
Empresa
   ↓
Estabelecimento
   ↓
Estacionamento
```

## RN-007 — Usuário pertence a uma empresa

Todo usuário operacional deverá estar associado a uma empresa.

**Status:** Proposta.

## RN-008 — Usuário pode possuir escopo de acesso

Um usuário poderá ter acesso:

- À empresa inteira;
- A determinados estabelecimentos;
- A determinados estacionamentos.

**Status:** Proposta.

---

# 11. Cliente

O cliente representa a pessoa ou organização que utiliza os serviços do estacionamento.

O cliente poderá ser:

- Pessoa física;
- Pessoa jurídica;
- Cliente avulso;
- Mensalista;
- Conveniado;
- Cliente pós-pago.

O cadastro deverá ser independente da operação de estacionamento.

---

# 12. Pessoa Física

Deverá permitir o cadastro de:

- Nome;
- CPF;
- Telefone;
- E-mail;
- Endereço;
- Outros dados necessários à operação.

---

# 13. Pessoa Jurídica

Deverá permitir:

- Razão social;
- Nome fantasia;
- CNPJ;
- Inscrição municipal;
- Inscrição estadual, quando aplicável;
- Endereço;
- Contatos.

---

# 14. Veículo

O veículo representa o automóvel utilizado na operação.

Informações possíveis:

- Placa;
- Marca;
- Modelo;
- Cor;
- Tipo;
- Cliente proprietário/responsável;
- Identificação adicional.

Um cliente poderá possuir vários veículos.

```text
Cliente
   1
   │
   N
Veículo
```

## RN-009 — Cliente pode possuir vários veículos

O cadastro de cliente não deverá limitar a quantidade de veículos associados.

**Status:** Proposta.

---

# 15. Operação de Estacionamento

A operação de estacionamento representa o ciclo de permanência de um veículo.

O fluxo principal será:

```text
Entrada
   ↓
Permanência
   ↓
Solicitação de saída
   ↓
Cálculo da tarifa
   ↓
Aplicação das regras
   ↓
Pagamento
   ↓
Saída
```

---

# 16. Entrada do Veículo

Ao entrar no estacionamento, deverá ser criada uma operação.

Informações mínimas:

- Estacionamento;
- Data/hora de entrada;
- Veículo;
- Placa;
- Operador;
- Ticket;
- Tipo de operação;
- Forma de identificação.

Poderão existir recursos como leitura automática de placa.

---

# 17. Ticket

O ticket representa a identificação da operação de estacionamento.

Poderá ser:

- Físico;
- Digital;
- Identificado pela placa;
- Identificado por código;
- Associado a equipamento.

O ticket deverá possuir um identificador único dentro do contexto definido pela operação.

---

# 18. Checklist de Avarias

A entrada poderá possuir registro de condições do veículo.

O checklist poderá registrar:

- Avarias;
- Fotos;
- Observações;
- Data/hora;
- Operador.

O checklist deverá permanecer associado à operação de estacionamento.

---

# 19. Permanência

Durante a permanência, a operação deverá permanecer em estado aberto.

Exemplo de estados:

```text
ABERTA
   ↓
EM_PERMANENCIA
   ↓
AGUARDANDO_PAGAMENTO
   ↓
PAGA
   ↓
FINALIZADA
```

Estados adicionais poderão ser necessários após validação do sistema existente.

---

# 20. Tipos de Operação

O sistema deverá suportar diferentes modalidades:

```text
ROTATIVO
MENSALISTA
CONVÊNIO
PÓS-PAGO
```

Cada modalidade poderá possuir regras próprias.

---

# 21. Rotativo

O rotativo representa o cliente que paga conforme o período de permanência.

A tarifação poderá considerar:

- Hora;
- Minuto;
- Fração;
- Diária;
- Pernoite;
- Tolerância;
- Promoção;
- Desconto.

**Fonte:** funcionalidades públicas do sistema de referência.

---

# 22. Mensalista

O mensalista possui uma condição comercial previamente definida.

Um contrato de mensalista poderá possuir:

- Cliente;
- Veículo(s);
- Estacionamento;
- Período de vigência;
- Valor;
- Horários permitidos;
- Dias permitidos;
- Vagas;
- Condições de cobrança;
- Situação do contrato.

---

# 23. Regra de Horário do Mensalista

O mensalista poderá possuir horários previamente contratados.

Caso a permanência ultrapasse as condições contratadas, poderá existir cobrança adicional como operação rotativa.

**Status:** Regra indicada pelo material público e deve ser validada no sistema existente.

---

# 24. Convênio

O convênio representa uma condição comercial entre o estacionamento e uma organização ou parceiro.

Exemplos:

- Shopping;
- Restaurante;
- Empresa;
- Hotel;
- Evento;
- Comércio.

Um convênio poderá definir:

- Conveniado;
- Estacionamento;
- Período de validade;
- Regra de desconto;
- Percentual;
- Valor fixo;
- Gratuidade;
- Limite de utilização.

---

# 25. Pós-Pago

O pós-pago permite que utilizações sejam acumuladas para cobrança posterior.

Fluxo:

```text
Utilização
   ↓
Registro
   ↓
Acúmulo
   ↓
Fechamento
   ↓
Fatura
   ↓
Vencimento
   ↓
Pagamento
```

Uma conta pós-paga poderá possuir:

- Cliente;
- Limite;
- Ciclo de faturamento;
- Data de vencimento;
- Utilizações;
- Faturas;
- Pagamentos;
- Situação financeira.

---

# 26. Tarifação

A tarifação é um dos principais componentes do domínio.

O cálculo deverá ser baseado em uma tabela de preços.

Conceitualmente:

```text
Operação
   ↓
Tipo de operação
   ↓
Tabela de preço
   ↓
Tempo de permanência
   ↓
Regras
   ↓
Descontos
   ↓
Valor final
```

---

# 27. Tabela de Preços

Uma tabela poderá possuir:

- Nome;
- Estacionamento;
- Vigência;
- Modalidade;
- Regras de cobrança;
- Valor;
- Fração;
- Intervalos;
- Tolerância;
- Períodos especiais.

A tabela deverá possuir histórico de alterações.

---

# 28. Vigência da Tarifação

Alterações de preço não deverão modificar operações já concluídas.

Uma operação deverá utilizar a configuração aplicável no momento definido pela regra de negócio.

**Status:** Proposta.

---

# 29. Tolerância

O sistema deverá permitir configuração de período de tolerância.

Exemplo:

```text
Entrada: 10:00
Tolerância: 15 minutos
Saída: 10:12

Resultado:
Valor = R$ 0,00
```

A tolerância deverá ser configurável por estacionamento/tabela de preço quando aplicável.

---

# 30. Desconto

O sistema poderá aplicar descontos sobre o valor calculado.

Tipos possíveis:

- Percentual;
- Valor fixo;
- Gratuidade;
- Convênio;
- Promoção;
- Regra comercial.

Todo desconto deverá possuir rastreabilidade.

---

# 31. Promoção

Promoções poderão definir condições especiais de cobrança.

Exemplos:

- Horário;
- Dia da semana;
- Data;
- Período;
- Cliente;
- Convênio;
- Evento.

---

# 32. Cálculo da Tarifa

O cálculo deverá considerar, no mínimo:

```text
Data/hora de entrada
+
Data/hora de saída
+
Tabela vigente
+
Modalidade
+
Tempo de permanência
+
Tolerância
+
Descontos
+
Convênios
+
Promoções
=
Valor final
```

O cálculo deverá ser determinístico e auditável.

---

# 33. Pagamento

O sistema deverá registrar pagamentos.

Formas possíveis:

- Dinheiro;
- Cartão;
- PIX;
- Outros meios suportados.

Cada pagamento deverá possuir:

- Valor;
- Data/hora;
- Método;
- Operador;
- Caixa;
- Referência da operação;
- Status.

---

# 34. Caixa

O caixa representa a unidade financeira da operação.

Deverá permitir:

```text
Abertura
   ↓
Movimentações
   ↓
Recebimentos
   ↓
Sangrias
   ↓
Suprimentos
   ↓
Fechamento
```

O fechamento deverá permitir conferência entre:

- Valor esperado;
- Valor informado;
- Diferença.

---

# 35. Faturamento

O sistema deverá suportar faturamento de operações e contratos.

Poderá contemplar:

- Mensalistas;
- Pós-pago;
- Empresas;
- Convênios;
- Serviços.

---

# 36. Contas a Receber

Deverá permitir:

- Faturas;
- Vencimentos;
- Pagamentos;
- Atrasos;
- Inadimplência;
- Cancelamentos;
- Baixas.

---

# 37. Nota Fiscal

O sistema poderá possuir integração com serviços de emissão de nota fiscal.

A emissão fiscal deverá ser desacoplada da regra principal do estacionamento.

Conceitualmente:

```text
Operação Financeira
       ↓
Documento Fiscal
       ↓
Integração Fiscal
```

---

# 38. Serviços Adicionais

O estacionamento poderá oferecer serviços adicionais.

Exemplos:

- Lava-rápido;
- Lavagem;
- Serviços automotivos;
- Produtos;
- Outros serviços.

Esses serviços deverão possuir domínio próprio, sem misturar sua estrutura com a operação de estacionamento.

---

# 39. Lava-Rápido

O serviço de lava-rápido poderá possuir:

- Serviço;
- Veículo;
- Cliente;
- Agendamento;
- Ordem de serviço;
- Funcionário;
- Valor;
- Pagamento;
- Status.

---

# 40. Agendamento

O sistema poderá permitir agendamento de serviços.

O agendamento deverá possuir:

- Cliente;
- Veículo;
- Serviço;
- Data/hora;
- Funcionário;
- Status.

Estados sugeridos:

```text
AGENDADO
   ↓
CONFIRMADO
   ↓
EM_EXECUCAO
   ↓
CONCLUIDO
```

Também deverão existir estados de cancelamento.

---

# 41. Estoque

O sistema poderá controlar produtos utilizados na operação.

Exemplos:

- Produtos de limpeza;
- Materiais;
- Insumos;
- Produtos vendidos.

Movimentações:

```text
Entrada
Saída
Ajuste
Inventário
```

---

# 42. Funcionários

O sistema poderá possuir cadastro de funcionários.

Informações:

- Nome;
- Documento;
- Contato;
- Cargo;
- Estabelecimento;
- Status;
- Data de admissão;
- Dados necessários para operação.

---

# 43. Comissionamento

Algumas operações poderão gerar comissão.

A comissão poderá estar relacionada a:

- Funcionário;
- Serviço;
- Venda;
- Operação;
- Percentual;
- Valor fixo.

O cálculo deverá ser independente do pagamento da comissão.

---

# 44. Relatórios

O sistema deverá permitir relatórios operacionais e administrativos.

Exemplos:

### Operacionais

- Entradas;
- Saídas;
- Veículos presentes;
- Ocupação;
- Permanência;
- Tickets abertos.

### Financeiros

- Recebimentos;
- Caixa;
- Faturamento;
- Contas a receber;
- Inadimplência.

### Comerciais

- Mensalistas;
- Convênios;
- Pós-pago;
- Descontos;
- Promoções.

### Gerenciais

- Receita;
- Ocupação;
- Ticket médio;
- Quantidade de veículos;
- Receita por período;
- Receita por estacionamento.

---

# 45. Auditoria

Operações relevantes deverão possuir rastreabilidade.

Deverá ser possível identificar:

- Quem realizou a operação;
- Quando;
- O que foi alterado;
- Valor anterior;
- Novo valor;
- Origem;
- Unidade operacional.

Eventos importantes:

- Alteração de preço;
- Cancelamento;
- Desconto;
- Estorno;
- Alteração de contrato;
- Alteração financeira;
- Abertura/fechamento de caixa;
- Alteração de permissões.

---

# 46. Regras de Negócio Consolidadas

## RN-001

Cada empresa representa um tenant isolado.

## RN-002

Uma empresa poderá possuir múltiplos estabelecimentos.

## RN-003

Um estabelecimento poderá representar matriz ou filial.

## RN-004

Um estabelecimento poderá possuir um ou mais estacionamentos.

## RN-005

Um estacionamento possui configurações operacionais próprias.

## RN-006

Usuários deverão possuir controle de acesso.

## RN-007

Um cliente poderá possuir múltiplos veículos.

## RN-008

Uma operação deverá registrar entrada e saída.

## RN-009

Uma operação deverá possuir uma modalidade.

## RN-010

A modalidade poderá ser rotativo, mensalista, convênio ou pós-pago.

## RN-011

A tarifa deverá ser calculada conforme tabela vigente.

## RN-012

O cálculo deverá considerar o período de permanência.

## RN-013

A tarifa poderá possuir tolerância.

## RN-014

A tarifa poderá receber descontos.

## RN-015

Convênios poderão alterar as condições de cobrança.

## RN-016

Mensalistas poderão possuir regras de horário.

## RN-017

Utilizações pós-pagas deverão compor uma conta/fatura.

## RN-018

Pagamentos deverão possuir rastreabilidade.

## RN-019

Movimentações financeiras deverão estar associadas a um contexto operacional.

## RN-020

Alterações relevantes deverão ser auditáveis.

## RN-021

Configurações com vigência deverão possuir histórico.

## RN-022

Uma alteração de configuração não deverá alterar retroativamente operações concluídas.

## RN-023

Dados de diferentes empresas não poderão ser misturados.

## RN-024

Usuários poderão possuir escopo limitado a determinadas unidades.

---

# 47. Modelo Conceitual do Domínio

O domínio inicial pode ser representado da seguinte forma:

```text
                    ┌───────────────┐
                    │    Empresa    │
                    └───────┬───────┘
                            │
                            │ 1:N
                            ▼
                  ┌───────────────────┐
                  │  Estabelecimento  │
                  │ Matriz / Filial   │
                  └─────────┬─────────┘
                            │
                            │ 1:N
                            ▼
                  ┌───────────────────┐
                  │   Estacionamento  │
                  └─────────┬─────────┘
                            │
              ┌─────────────┼──────────────┐
              │             │              │
              ▼             ▼              ▼
          Tarifação      Usuários       Operações
                                         │
                                         ▼
                                      Veículo
                                         │
                                         ▼
                                       Cliente
```

---

# 48. Núcleo da Operação

O agregado conceitual principal deverá representar:

```text
ParkingOperation
│
├── Vehicle
├── Customer
├── Entry
├── Ticket
├── Pricing
├── Discounts
├── Agreement
├── Payment
├── Exit
└── Audit
```

A implementação final poderá separar esses conceitos em agregados diferentes conforme a evolução da modelagem de domínio.

---

# 49. Estados da Operação

Estados preliminares:

```text
ABERTA
EM_PERMANENCIA
AGUARDANDO_PAGAMENTO
PAGA
FINALIZADA
CANCELADA
```

Os estados definitivos deverão ser validados durante a implementação.

---

# 50. Integridade Temporal

O domínio possui forte dependência de tempo.

Devem ser considerados:

- Entrada;
- Saída;
- Vigência de tabela;
- Vigência de contrato;
- Vigência de convênio;
- Data de vencimento;
- Data de pagamento;
- Data de faturamento.

Consequentemente, entidades que representam configurações comerciais deverão possuir controle de vigência.

---

# 51. Requisitos Não Funcionais

## RNF-001 — Segurança

O sistema deverá possuir autenticação e autorização.

## RNF-002 — Isolamento

Os tenants deverão possuir isolamento lógico.

## RNF-003 — Auditoria

Operações críticas deverão ser rastreáveis.

## RNF-004 — Disponibilidade

A operação de estacionamento deverá continuar funcionando mesmo diante de indisponibilidade temporária de serviços externos, quando tecnicamente possível.

## RNF-005 — Performance

Operações de entrada, consulta e saída deverão possuir baixa latência.

## RNF-006 — Escalabilidade

A arquitetura deverá permitir crescimento do número de empresas, estabelecimentos e operações.

## RNF-007 — Observabilidade

A aplicação deverá possuir logs, métricas e rastreamento de erros.

## RNF-008 — Integrações

Integrações externas deverão ser desacopladas do domínio principal.

---

# 52. Operação Offline

Considerando a natureza operacional do estacionamento, indisponibilidade de internet não deverá necessariamente impedir a operação.

A arquitetura deverá avaliar a possibilidade de:

```text
Operação Local
      ↓
Persistência temporária
      ↓
Reconexão
      ↓
Sincronização
```

Essa regra deverá ser detalhada posteriormente.

**Status:** A validar.

---

# 53. Integrações

As integrações serão tratadas como uma camada externa ao domínio.

Possíveis integrações:

- Sistemas de estacionamento existentes;
- Equipamentos;
- Leitores de placa;
- Terminais;
- Meios de pagamento;
- Sistemas fiscais;
- Sistemas financeiros;
- Serviços de mensagens;
- APIs externas.

---

# 54. Engenharia Reversa do Sistema Existente

A integração com o sistema existente deverá ocorrer somente após a definição do domínio deste sistema.

A análise deverá buscar identificar:

```text
Nosso Domínio
       │
       │
       ▼
Mapeamento
       │
       ▼
Domínio Existente
```

Não deverá ser assumido que os conceitos possuem os mesmos nomes ou estruturas.

Exemplo:

```text
Nosso sistema:
ParkingOperation

Sistema existente:
Ticket / Movimento / Atendimento
```

Os conceitos deverão ser mapeados explicitamente.

---

# 55. Estratégia de Integração

A integração deverá preferencialmente utilizar uma camada de tradução:

```text
┌─────────────────────────┐
│     Nosso Sistema       │
│                         │
│     Nosso Domínio       │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│ Integration / Adapter   │
│                         │
│ Mapping / Translation   │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│   Sistema Existente     │
└─────────────────────────┘
```

O domínio principal não deverá depender diretamente dos modelos internos do sistema legado.

---

# 56. Anti-Corruption Layer

Caso a integração com o sistema existente utilize conceitos incompatíveis com o domínio proposto, deverá ser considerada a implementação de uma **Anti-Corruption Layer**.

Responsabilidades:

- Conversão de modelos;
- Conversão de identificadores;
- Conversão de status;
- Conversão de regras;
- Tratamento de erros;
- Idempotência;
- Sincronização.

---

# 57. Identificadores

Cada entidade deverá possuir identificador próprio.

Não deverá ser assumido que o identificador do sistema externo será utilizado como identificador interno.

Quando necessário:

```text
InternalId
ExternalId
ExternalSystem
```

poderão ser mantidos separadamente.

---

# 58. Idempotência

Integrações deverão ser idempotentes sempre que possível.

Uma mesma mensagem ou evento não deverá produzir múltiplas operações indevidas.

Exemplo:

```text
Evento externo
     │
     ▼
Processamento
     │
     ▼
Registro de Idempotência
     │
     ├── Já processado → Ignorar
     │
     └── Novo → Processar
```

---

# 59. Eventos de Domínio

Eventos poderão ser utilizados para desacoplar processos.

Exemplos:

```text
VehicleEntered
VehicleExited
TicketCreated
PaymentCompleted
ParkingOperationCompleted
InvoiceIssued
MonthlyContractCreated
MonthlyContractExpired
PostpaidInvoiceGenerated
CashRegisterOpened
CashRegisterClosed
```

Os eventos definitivos deverão ser definidos durante a modelagem do domínio.

---

# 60. Auditoria e Eventos

Eventos de domínio e auditoria não deverão ser tratados como a mesma coisa.

### Evento de domínio

Representa algo importante que aconteceu no negócio.

### Auditoria

Representa o histórico de quem realizou uma ação ou alterou um dado.

Exemplo:

```text
Evento:
PaymentCompleted

Auditoria:
Usuário X alterou a operação Y às 15:32.
```

---

# 61. Premissas para a Modelagem do Banco

O banco deverá ser modelado somente após a validação deste documento.

A modelagem deverá respeitar:

1. Regras de negócio;
2. Relacionamentos do domínio;
3. Cardinalidades;
4. Histórico;
5. Vigência;
6. Auditoria;
7. Multi-tenancy;
8. Integridade;
9. Idempotência;
10. Integrações.

---

# 62. Cardinalidades Iniciais

As cardinalidades preliminares são:

```text
Empresa
1:N
Estabelecimento

Estabelecimento
1:N
Estacionamento

Empresa
1:N
Usuário

Cliente
1:N
Veículo

Estacionamento
1:N
Operação

Veículo
1:N
Operação

Operação
1:N
Pagamento

Estacionamento
1:N
Tabela de Preço

Cliente
1:N
Contrato

Cliente
1:N
Fatura
```

Essas cardinalidades são preliminares e poderão sofrer alterações durante a modelagem.

---

# 63. Entidades Conceituais Iniciais

As seguintes entidades deverão ser consideradas na próxima fase:

### Organização

- Company
- Establishment
- Parking

### Segurança

- User
- Role
- Permission
- UserScope

### Pessoas

- Customer
- Person
- CompanyCustomer
- Employee

### Veículos

- Vehicle

### Operação

- ParkingOperation
- Ticket
- Entry
- Exit
- VehicleInspection

### Tarifação

- PricingTable
- PricingRule
- Tolerance
- Discount
- Promotion

### Comercial

- MonthlyContract
- Agreement
- PostpaidAccount

### Financeiro

- Payment
- CashRegister
- CashMovement
- Invoice
- Receivable

### Fiscal

- TaxConfiguration
- FiscalDocument

### Serviços

- Service
- Appointment
- WorkOrder

### Estoque

- Product
- Stock
- StockMovement

### Auditoria

- AuditLog

### Integração

- ExternalSystem
- ExternalReference
- IntegrationEvent
- IdempotencyRecord

---

# 64. O que ainda não deve ser definido

Neste estágio ainda não deverão ser definidos:

- Nome definitivo das tabelas;
- Tipos de colunas;
- Índices;
- Foreign Keys definitivas;
- Estratégia física de particionamento;
- Stored Procedures;
- Triggers;
- Views;
- Estrutura definitiva de APIs.

Essas decisões serão tomadas após a conclusão da análise de domínio.

---

# 65. Pontos a Validar

Os seguintes pontos deverão ser validados posteriormente:

- Quantidade máxima de estabelecimentos;
- Relação entre estabelecimento e estacionamento;
- Possibilidade de múltiplos estacionamentos por filial;
- Regras exatas de mensalista;
- Regras de horário;
- Regras de tolerância;
- Regras de pernoite;
- Regras de diária;
- Regras de fracionamento;
- Regras de convênio;
- Regras de pós-pago;
- Regras de faturamento;
- Regras de inadimplência;
- Regras de desconto;
- Regras de promoção;
- Regras de cancelamento;
- Regras de estorno;
- Regras de fechamento de caixa;
- Operação offline;
- Leitura de placas;
- Integrações com equipamentos;
- Integração fiscal;
- Integração com o sistema existente.

---

# 66. Matriz de Status dos Requisitos

| Requisito | Descrição | Origem | Status |
|---|---|---|---|
| REQ-001 | Sistema multiempresa | Arquitetura | Proposto |
| REQ-002 | Empresa pode possuir matriz/filiais | Modelo organizacional | Proposto |
| REQ-003 | Estabelecimento pode possuir estacionamento | Modelo de domínio | Proposto |
| REQ-004 | Controle de operação de estacionamento | Sistema de referência | Confirmado |
| REQ-005 | Operação rotativa | Sistema de referência | Confirmado |
| REQ-006 | Mensalistas | Sistema de referência | Confirmado |
| REQ-007 | Convênios | Sistema de referência | Confirmado |
| REQ-008 | Pós-pago | Sistema de referência | Confirmado |
| REQ-009 | Tabelas de preço | Sistema de referência | Confirmado |
| REQ-010 | Descontos | Sistema de referência | Confirmado |
| REQ-011 | Tolerância | Sistema de referência | Confirmado |
| REQ-012 | Controle de caixa | Sistema de referência | Confirmado |
| REQ-013 | Serviços adicionais | Sistema de referência | Confirmado |
| REQ-014 | Lava-rápido | Sistema de referência | Confirmado |
| REQ-015 | Faturamento | Sistema de referência | Confirmado |
| REQ-016 | NFS-e | Sistema de referência | Confirmado |
| REQ-017 | Auditoria | Arquitetura | Proposto |
| REQ-018 | Controle granular de acesso | Arquitetura | Proposto |
| REQ-019 | Operação offline | Necessidade operacional | A validar |
| REQ-020 | Integração com sistema existente | Escopo do projeto | Futuro |

---

# 67. Fluxo Geral do Sistema

```text
                    ┌───────────────┐
                    │    EMPRESA    │
                    └───────┬───────┘
                            │
                            ▼
                    ┌───────────────┐
                    │ESTABELECIMENTO│
                    └───────┬───────┘
                            │
                            ▼
                    ┌───────────────┐
                    │ ESTACIONAMENTO│
                    └───────┬───────┘
                            │
                            ▼
                     ┌─────────────┐
                     │    ENTRADA  │
                     └──────┬──────┘
                            │
                            ▼
                     ┌─────────────┐
                     │ PERMANÊNCIA │
                     └──────┬──────┘
                            │
                            ▼
                     ┌─────────────┐
                     │   TARIFA    │
                     └──────┬──────┘
                            │
                  ┌─────────┼─────────┐
                  ▼         ▼         ▼
              ROTATIVO  MENSALISTA CONVÊNIO
                  │         │         │
                  └─────────┼─────────┘
                            │
                            ▼
                     ┌─────────────┐
                     │  PAGAMENTO  │
                     └──────┬──────┘
                            │
                            ▼
                     ┌─────────────┐
                     │    SAÍDA    │
                     └──────┬──────┘
                            │
                            ▼
                     ┌─────────────┐
                     │  AUDITORIA  │
                     └─────────────┘
```

---

# 68. Diretriz para a Próxima Fase

Após a aprovação deste documento, a próxima fase será a **Modelagem de Domínio**.

A sequência recomendada será:

```text
FASE 1
Análise de Requisitos
        ↓
FASE 2
Modelagem do Domínio
        ↓
FASE 3
Casos de Uso
        ↓
FASE 4
Regras de Negócio Detalhadas
        ↓
FASE 5
Modelo Conceitual de Dados
        ↓
FASE 6
Modelo Lógico de Dados
        ↓
FASE 7
Modelo Físico do Banco
        ↓
FASE 8
Arquitetura da Aplicação
        ↓
FASE 9
Implementação
        ↓
FASE 10
Engenharia Reversa do Sistema Existente
        ↓
FASE 11
Camada de Integração
```

---

# 69. Critério para Aprovação do Documento

Este documento será considerado pronto para a próxima fase quando:

- Os principais atores estiverem identificados;
- Os principais processos estiverem identificados;
- As principais entidades estiverem identificadas;
- As regras críticas estiverem documentadas;
- As cardinalidades principais estiverem compreendidas;
- Os pontos de dúvida estiverem registrados;
- As premissas estiverem separadas das regras confirmadas;
- O modelo multiempresa estiver validado;
- A relação Empresa → Estabelecimento → Estacionamento estiver validada;
- Os fluxos de entrada e saída estiverem compreendidos;
- As modalidades de cobrança estiverem identificadas.

---

# 70. Conclusão

O sistema deverá ser concebido como uma plataforma de gestão de estacionamentos orientada ao domínio, onde a operação de estacionamento representa o núcleo do negócio.

A estrutura organizacional deverá separar claramente:

```text
Empresa
   ↓
Estabelecimento
   ↓
Estacionamento
```

Essa separação permitirá suportar cenários de empresas com matriz, múltiplas filiais e múltiplas unidades operacionais sem criar dependências desnecessárias no modelo.

O domínio operacional deverá ser organizado em torno do ciclo:

```text
Entrada
   ↓
Permanência
   ↓
Tarifação
   ↓
Pagamento
   ↓
Saída
```

com suporte às modalidades:

```text
Rotativo
Mensalista
Convênio
Pós-pago
```

A modelagem de dados deverá ser derivada dessas regras e não das interfaces do sistema.

A integração com o sistema existente será tratada posteriormente, através de uma camada de integração responsável por traduzir os conceitos entre os dois sistemas.

O objetivo final é que o novo sistema possua um **domínio próprio, coerente e independente**, permitindo que o sistema existente seja integrado sem determinar a arquitetura interna da nova solução.

---

## Referências

- Jump Park — Sistema de gestão para estacionamentos: https://www.jumppark.com.br/
- Jump Park — Funcionalidades e aplicativo operacional: https://www.jumppark.com.br/funcionalidades/aplicativo-operacional
- Jump Park — Gestão de pós-pago: https://www.jumppark.com.br/funcionalidades/gestao-de-pos-pago
- Jump Park — Gestão de estacionamento rotativo: https://www.jumppark.com.br/blog/tudo-sobre-controle-de-estacionamento-rotativo
- Jump Park — Sistema administrativo: https://jumppark.com.br/sistema-administrativo-estacionamento/

---

## Observação sobre Engenharia Reversa

Este documento não representa uma especificação interna ou oficial..

As funcionalidades públicas utilizadas como referência servem exclusivamente como base para abstração de requisitos de negócio para este projeto.

Regras internas, algoritmos, estruturas de dados, integrações e comportamentos não publicados deverão ser tratados como **a validar** e não deverão ser assumidos como fatos até que sejam confirmados.
