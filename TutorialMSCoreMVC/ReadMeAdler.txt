https://docs.microsoft.com/pt-br/aspnet/core/data/ef-mvc/intro?view=aspnetcore-2.1
https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-2.1




Para adicionar o suporte do EF Core a um projeto, instale o provedor de banco de dados que você deseja ter como destino. 
Este tutorial usa o SQL Server e o pacote de provedor é Microsoft.EntityFrameworkCore.SqlServer. 
Este pacote está incluído no metapacote Microsoft.AspNetCore.All e, portanto, não é necessário instalá-lo.

************
Em seguida, você usará o mecanismo de scaffolding no Visual Studio para adicionar um 
controlador MVC e exibições que usam o EF para consultar e salvar dados.

Clique com o botão direito do mouse na pasta Controladores no Gerenciador de Soluções e selecione Adicionar > Novo Item Gerado por Scaffolding.
Na caixa de diálogo Adicionar Scaffolding:
Selecione Controlador MVC com exibições, usando o Entity Framework.
Clique em Adicionar.
Na caixa de diálogo Adicionar Controlador:
Na classe Model, selecione Aluno.
Na Classe de contexto de dados selecione SchoolContext.
Aceite o StudentsController padrão como o nome.
Clique em Adicionar.

********************
Observação de segurança sobre o excesso de postagem
O atributo Bind que o código gerado por scaffolding inclui no método Create é uma maneira de proteger contra o 
excesso de postagem em cenários de criação. Por exemplo, suponha que a entidade Student inclua uma propriedade 
Secret que você não deseja que essa página da Web defina.
public class Student
{
    public int ID { get; set; }
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string Secret { get; set; }
}

***********************************
A abordagem "criar e anexar" para HttpPost Delete
Se a melhoria do desempenho de um aplicativo de alto volume for uma prioridade, 
você poderá evitar uma consulta SQL desnecessária criando uma instância de uma entidade 
Student usando somente o valor de chave primária e, em seguida, definindo o estado da entidade como Deleted. 
Isso é tudo o que o Entity Framework precisa para excluir a entidade. 
(Não coloque esse código no projeto; ele está aqui apenas para ilustrar uma alternativa

Student studentToDelete = new Student() { ID = id };
_context.Entry(studentToDelete).State = EntityState.Deleted;
await _context.SaveChangesAsync();

Se a entidade tiver dados relacionados, eles também deverão ser excluídos. 
Verifique se a exclusão em cascata está configurada no banco de dados. Com essa abordagem para a exclusão de entidade, 
o EF talvez não perceba que há entidades relacionadas a serem excluídas.

4 - Migrations

Pacotes NuGet do Entity Framework Core para migrações
Para trabalhar com migrações, use o PMC (Console do Gerenciador de Pacotes) ou a CLI (interface de linha de comando). Esses tutoriais mostram como usar comandos da CLI. Encontre informações sobre o PMC no final deste tutorial.
As ferramentas do EF para a CLI (interface de linha de comando) são fornecidas em Microsoft.EntityFrameworkCore.Tools.DotNet. Para instalar esse pacote, adicione-o à coleção DotNetCliToolReference no arquivo .csproj, conforme mostrado. Observação: é necessário instalar este pacote editando o arquivo .csproj; não é possível usar o comando install-package ou a GUI do Gerenciador de Pacotes. Edite o arquivo .csproj clicando com o botão direito do mouse no nome do projeto no Gerenciador de Soluções e selecionando Editar ContosoUniversity.csproj.

<ItemGroup>
  <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
  <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
</ItemGroup>

Caso eu queira excluir o banco de dados
-> dotnet ef database drop
Creating a migration
-> dotnet ef migrations add InitialCreate
-> Excluir a ultima migração
dotnet ef migrations remove

/******
Nenhum comando "dotnet-ef" executável correspondente encontrado, consulte esta postagem no blog para ajudar a solucionar o problema.
Troubleshooting the dotnet ef command for EF Core Migrations
http://thedatafarm.com/data-access/no-executable-found-matching-command-dotnet-ef/
*****/


5 - Data Models
https://docs.microsoft.com/pt-br/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-2.1#the-input-tag-helper
A configuração ApplyFormatInEditMode especifica que a formatação também deve ser aplicada quando o valor é exibido em uma caixa de texto para edição. (Talvez você não deseje ter isso em alguns campos – por exemplo, para valores de moeda, provavelmente, você não deseja exibir o símbolo de moeda na caixa de texto para edição.)

public ICollection<CourseAssignment> CourseAssignments { get; set; }
Se uma propriedade de navegação pode conter várias entidades, o tipo precisa ser uma lista na qual as entradas podem ser adicionadas, excluídas e atualizadas. Especifique ICollection<T> ou um tipo, como List<T> ou HashSet<T>. Se você especificar ICollection<T>, o EF criará uma coleção HashSet<T> por padrão.

O Entity Framework não exige que você adicione uma propriedade de chave estrangeira ao modelo de dados quando você tem uma propriedade de navegação para uma entidade relacionada. O EF cria chaves estrangeiras no banco de dados sempre que elas são necessárias e cria automaticamente propriedades de sombra para elas. No entanto, ter a chave estrangeira no modelo de dados pode tornar as atualizações mais simples e mais eficientes. Por exemplo, quando você busca uma entidade de curso a ser editada, a entidade Department é nula se você não carregá-la; portanto, quando você atualiza a entidade de curso, você precisa primeiro buscar a entidade Department. Quando a propriedade de chave estrangeira DepartmentID está incluída no modelo de dados, você não precisa buscar a entidade Department antes da atualização.

Por convenção, o Entity Framework habilita a exclusão em cascata para chaves estrangeiras que não permitem valor nulo e em relações muitos para muitos. Isso pode resultar em regras de exclusão em cascata circular, que causará uma exceção quando você tentar adicionar uma migração. Por exemplo, se você não definiu a propriedade Department.InstructorID como uma propriedade que permite valor nulo, o EF configura uma regra de exclusão em cascata para excluir o instrutor quando você exclui o departamento, que não é o que você deseja que aconteça. Se as regras de negócio exigissem que a propriedade InstructorID não permitisse valor nulo, você precisaria usar a seguinte instrução de API fluente para desabilitar a exclusão em cascata na relação:
modelBuilder.Entity<Department>()
   .HasOne(d => d.Administrator)
   .WithMany()
   .OnDelete(DeleteBehavior.Restrict)


Chave composta
Como as chaves estrangeiras não permitem valor nulo e, juntas, identificam exclusivamente cada linha da tabela, não é necessário ter uma chave primária. As propriedades InstructorID e CourseID devem funcionar como uma chave primária composta. A única maneira de identificar chaves primárias compostas no EF é usando a API fluente (isso não pode ser feito por meio de atributos). Você verá como configurar a chave primária composta na próxima seção.
A chave composta garante que, embora você possa ter várias linhas para um curso e várias linhas para um instrutor, não poderá ter várias linhas para o mesmo instrutor e curso. A entidade de junção Enrollment define sua própria chave primária e, portanto, duplicatas desse tipo são possíveis. Para evitar essas duplicatas, você pode adicionar um índice exclusivo nos campos de chave estrangeira ou configurar Enrollment com uma chave primária composta semelhante a CourseAssignment. Para obter mais informações, consulte Índices.
https://docs.microsoft.com/ef/core/modeling/indexes


7 - Criar, Editar

Instrutores
*Adicionar uma página Editar para instrutores
Quando você edita um registro de instrutor, deseja poder atualizar a atribuição de escritório do instrutor. A entidade Instructor tem uma relação um para zero ou um com a entidade OfficeAssignment, o que significa que o código deve manipular as seguintes situações:
Se o usuário apagar a atribuição de escritório e ela originalmente tinha um valor, exclua a entidade OfficeAssignment.
Se o usuário inserir um valor de atribuição de escritório e ele originalmente estava vazio, crie uma nova entidade OfficeAssignment.
Se o usuário alterar o valor de uma atribuição de escritório, altere o valor em uma entidade OfficeAssignment existente.

*Adicionar atribuições de Curso à página Editar Instrutor
A relação entre as entidades Course e Instructor é muitos para muitos. Para adicionar e remover relações, adicione e remova entidades bidirecionalmente no conjunto de entidades de junção CourseAssignments.

10 - Topicos Avançados
Use o método DbSet.FromSql para consultas que retornam tipos de entidade. Os objetos retornados precisam ser do tipo esperado pelo objeto DbSet e são controlados automaticamente pelo contexto de banco de dados, a menos que você desative o controle.
Use o Database.ExecuteSqlCommand para comandos que não sejam de consulta.
Caso precise executar uma consulta que retorna tipos que não são entidades, use o ADO.NET com a conexão de banco de dados fornecida pelo EF. Os dados retornados não são controlados pelo contexto de banco de dados, mesmo se esse método é usado para recuperar tipos de entidade.