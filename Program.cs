using LibGit2Sharp;

//Adicionar o caminho onde está o repositório git localmente
string repositorioLocal = "C:\\Users\\Jeniffer\\Documents\\Cursos Alura\\C#Web-curso1-final";

//Inserir o nome da branch a ser consultada
string nomeBranch =  "aula03-generics";

//Nome do arquivo a ser salvo (incluir o caminho caso queira salvar em outro local)
string arquivoMarkdown = "aula01.md";

ExportarCommitsParaMarkdown(repositorioLocal, nomeBranch, arquivoMarkdown);

static void ExportarCommitsParaMarkdown(string repositorioLocal, string nomeBranch, string arquivoMarkdown)
{
    using (var repo = new Repository(repositorioLocal))
    {
        using (StreamWriter writer = new StreamWriter(arquivoMarkdown))
        {
            var branch = repo.Branches[nomeBranch];
            if (branch != null)
            {
                    writer.WriteLine("# Ementa detalhada com códigos por vídeo");
                    writer.WriteLine($"# Commits da Branch {nomeBranch}");
                    writer.WriteLine();

                    var commits = branch.Commits.OrderByDescending(c => c.Committer.When);

                    foreach (var commit in commits)
                        {
                            writer.WriteLine($"# {commit.Sha.Substring(0, 7)} - {commit.Message}");

                            var parents = commit.Parents.ToArray();

                            var patches = repo.Diff.Compare<Patch>(parents.Length > 0 ? parents[0].Tree : null, commit.Tree);

                            foreach (var patch in patches)
                            {
                                writer.WriteLine($" ## {patch.Status} {patch.Path}");
                                writer.WriteLine($"```{patch.Patch}```");
                                writer.WriteLine();
                            }

                                writer.WriteLine();
                    }

                    Console.WriteLine($"Commits exportados para {arquivoMarkdown}");

            }
            else
            {
                Console.WriteLine($"A branch {nomeBranch} não existe no repositório.");
            }
            
            
        }
    }
}

    