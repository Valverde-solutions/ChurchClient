## Como usar este projeto com o agente Codex

1. **Leia este arquivo antes de qualquer tarefa.**  
   - Sempre valide aqui a estrutura de pastas, soluções e camadas antes de propor mudanças.

2. **Build principal**  
   - Use: `dotnet build src/ChurchSaaS.Admin.sln`  
   - Não altere o nome nem a localização da solution sem alinhamento explícito do usuário.

3. **Arquitetura e camadas (DDD / Clean Architecture)**  
   - `ChurchSaaS.Admin.Domain`  
     - Contém o domínio (entidades, value objects, agregados, eventos de domínio, services).  
     - **Não deve referenciar** nenhum outro projeto (especialmente `Infrastructure` e `Api`).
   - `ChurchSaaS.Admin.Application`  
     - Contém casos de uso (commands/queries), DTOs, interfaces e orquestração de regras de negócio.  
     - Pode depender **apenas** de `Domain`.
   - `ChurchSaaS.Admin.Infrastructure`  
     - Implementa acesso a dados, repositórios, configurações de persistência, etc.  
     - Pode depender de `Application` e `Domain`, mas **Domain nunca depende de Infrastructure**.
   - `ChurchSaaS.Admin.Api`  
     - Ponto de entrada da API (Minimal API/Controllers, middlewares, configs).  
     - Depende de `Application` (e indiretamente de `Domain` via `Application`).
   - `ChurchSaaS.Admin.BlazorApp`  
     - UI do backoffice. Depende de `Application` (para casos de uso) e **não** deve acessar `Infrastructure` diretamente.

4. **UI / Blazor**  
   - Estilos principais: `src/ChurchSaaS.Admin.BlazorApp/wwwroot/css/site.css`  
   - Layout principal: `src/ChurchSaaS.Admin.BlazorApp/Shared/MainLayout.razor`  
   - Não crie componentes complexos ou regras de negócio no código-behind sem solicitação.  
   - A lógica de negócio deve continuar nas camadas `Application` / `Domain`.

5. **API**  
   - Entrada: `src/ChurchSaaS.Admin.Api/Program.cs`  
   - Configurações principais: `src/ChurchSaaS.Admin.Api/appsettings.json`  
   - Ao criar endpoints:
     - Mantenha-os finos (sem regras de negócio direto no endpoint).
     - Delegue para serviços/casos de uso na camada `Application`.
   - Não exponha diretamente tipos de `Infrastructure` para o mundo externo.

6. **O que o Codex pode fazer por padrão**  
   - Criar/ajustar:
     - Estrutura de pastas dentro dos projetos.
     - Arquivos básicos de inicialização (por exemplo, `Program.cs`, endpoints mínimos, componentes Blazor simples).
     - Configurações de projeto (`.csproj`, `appsettings.json`) quando solicitado.
   - **Não criar automaticamente**:
     - Entidades de domínio, agregados, value objects, eventos de domínio ou regras de negócio sem pedido explícito.  
     - Padrões complexos (MediatR, EF Core, CQRS completo, etc.) sem instrução do usuário.

7. **Integração com outros sistemas**  
   - Considere que o módulo de Produto (ChurchSaaS.Product) está em outro repositório e se comunica via **API HTTP**.  
   - Não referencie projetos externos ou paths relativos de outros repositórios.  
   - Quando precisar integrar, use `HttpClient` e contratos locais, conforme instruções do usuário.

8. **Não remover ou reverter alterações existentes sem solicitação do usuário.**  
   - Não apague arquivos, projetos, referências ou configurações existentes por conta própria.  
   - Qualquer refactor estrutural (renomear/mover projetos, mudar namespaces globais, etc.) deve ser solicitado explicitamente.

9. **Evite comandos destrutivos**  
   - Não use comandos como:
     - `git reset --hard`
     - `git checkout --`
     - `rm -rf`
   - Nenhuma operação que possa causar perda de código ou histórico deve ser sugerida ou executada sem instrução explícita do usuário.

10. **Estilo e princípios (SOLID / DDD)**  
   - Prefira coesão alta e acoplamento baixo entre classes e camadas.  
   - Mantenha regras de negócio fora de controllers/endpoints e componentes de UI.  
   - Qualquer decisão de arquitetura maior (novos projetos, bibliotecas adicionais, mudança de padrão) deve ser validada com o usuário antes.
