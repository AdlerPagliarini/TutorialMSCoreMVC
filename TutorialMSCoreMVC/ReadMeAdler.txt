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





