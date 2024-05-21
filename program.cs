using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NAudio.Wave;

namespace Floricultura
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Inicialização do banco de dados
            var client = new MongoClient("mongodb+srv://davibd:davi123@davi-bdzenior.cuufxqb.mongodb.net/?retryWrites=true&w=majority&appName=davi-bdZenior");
            var database = client.GetDatabase("davibd");
            var collection = database.GetCollection<Planta>("floriculturaDaniel");

            
           

            // Menu da Floricultura
            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("========== Floricultura ==========");
                Console.WriteLine("1. Adicionar planta");
                Console.WriteLine("2. Buscar planta por Nome");
                Console.WriteLine("3. Listar todos as plantas");
                Console.WriteLine("4. Atualizar planta");
                Console.WriteLine("5. Deletar planta");
                Console.WriteLine("6. Sair");
                Console.WriteLine("=============================");
                Console.Write("Escolha uma opção: ");
                var opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        await AdicionarPlanta(collection);
                        break;
                    case "2":
                        await BuscarPlanta(collection);
                        break;
                    case "3":
                        await ListarTodasPlantas(collection);
                        break;
                    case "4":
                        await AtualizarPlanta(collection);
                        break;
                    case "5":
                        await DeletarPlanta(collection);
                        break;
                    case "6":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static async Task AdicionarPlanta(IMongoCollection<Planta> collection)
        {
            Console.Write("Nome cientifico da planta: ");
            var nomeCientifico = Console.ReadLine();

            Console.Write("Nome comum da planta: ");
            var nomeComum = Console.ReadLine();

            Console.Write("Familia da planta: ");
            var familia = Console.ReadLine();

            Console.Write("Origem da planta: ");
            var origem = Console.ReadLine();

            Console.Write("Descrição: ");
            var descricao = Console.ReadLine();

            Console.Write("URL da imagem: ");
            var urlImagem = Console.ReadLine();


            var planta = new Planta
            {
                NomeCientifico = nomeCientifico,
                NomeComum = nomeComum,
                Familia = familia,
                Origem = origem,
                Descricao = descricao,
                UrlImagem = urlImagem
            };

            await collection.InsertOneAsync(planta);
            Console.WriteLine("Planta adicionada com sucesso.");         
        }

        static async Task BuscarPlanta(IMongoCollection<Planta> collection)
        {
            Console.Write("Digite o nome cientifico da planta: ");
            var nome = Console.ReadLine();

            var filter = Builders<Planta>.Filter.Eq(p => p.NomeCientifico, nome);
            var planta = await collection.Find(filter).FirstOrDefaultAsync();

            if (planta != null)
            {
                Console.WriteLine($"Planta encontrada:");
                Console.WriteLine($"Nome Cientifico: {planta.NomeCientifico}");
                Console.WriteLine($"Nome Comum: {planta.NomeComum}");
                Console.WriteLine($"Familia: {planta.Familia}");
                Console.WriteLine($"Origem: {planta.Origem}");
                Console.WriteLine($"Descrição: {planta.Descricao}");
                Console.WriteLine($"URL Imagem: {planta.UrlImagem}");
                
            }
            else
            {
                Console.WriteLine("Planta não encontrado.");
            }
        }

        static async Task ListarTodasPlantas(IMongoCollection<Planta> collection)
        {
            var plantas = await collection.Find(_ => true).ToListAsync(); 

            foreach (var planta in plantas)
            {
                Console.WriteLine($"Nome Cientifico: {planta.NomeCientifico}");
                Console.WriteLine($"Nome Comum: {planta.NomeComum}");
                Console.WriteLine($"Familia: {planta.Familia}");
                Console.WriteLine($"Origem: {planta.Origem}");
                Console.WriteLine($"Descrição: {planta.Descricao}");
                Console.WriteLine($"URL Imagem: {planta.UrlImagem}");

                Console.WriteLine();
            }
        }

        static async Task AtualizarPlanta(IMongoCollection<Planta> collection)
        {
            Console.Write("Digite o nome cientifico do planta que deseja atualizar: ");
            var nome = Console.ReadLine();

            var filter = Builders<Planta>.Filter.Eq(p => p.NomeCientifico, nome);
            var planta = await collection.Find(filter).FirstOrDefaultAsync();

            if (planta != null)
            {
                Console.Write("Novo Nome cientifico da planta: ");
                var novoNomeCientifico = Console.ReadLine();

                Console.Write("Novo Nome comum da planta: ");
                var novoNomeComum = Console.ReadLine();

                Console.Write("Nova Familia da planta: ");
                var novaFamilia = Console.ReadLine();

                Console.Write("Nova Origem da planta: ");
                var novaOrigem = Console.ReadLine();

                Console.Write("Nova Descrição: ");
                var novaDescricao = Console.ReadLine();

                Console.Write("Nova URL da imagem: ");
                var novaUrlImagem = Console.ReadLine();
                
                var update = Builders<Planta>.Update.Set(p => p.NomeCientifico, novoNomeCientifico)
                                                     .Set(p => p.NomeComum, novoNomeComum)
                                                     .Set(p => p.Familia, novaFamilia)
                                                     .Set(p => p.Origem, novaOrigem)
                                                     .Set(p => p.Descricao, novaDescricao)
                                                     .Set(p => p.UrlImagem, novaUrlImagem);

                await collection.UpdateOneAsync(filter, update);
                Console.WriteLine("Planta atualizado com sucesso.");
            }
            else
            {
                Console.WriteLine("Planta não encontrado.");
            }
        }

        static async Task DeletarPlanta(IMongoCollection<Planta> collection)
        {
            Console.Write("Digite o nome cientifico da planta que deseja deletar: ");
            var nome = Console.ReadLine();

            var filter = Builders<Planta>.Filter.Eq(p => p.NomeCientifico, nome);
            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
            {
                Console.WriteLine("Planta deletado com sucesso.");
            }
            else
            {
                Console.WriteLine("Planta não encontrado.");
            }
        }

    }
}
public class Planta
    {
        public ObjectId Id { get; set; }
        public string NomeCientifico { get; set; }
        public string NomeComum { get; set; }
        public string Familia { get; set; }
        public string Origem { get; set; }
        public string Descricao { get; set; }
        public string UrlImagem { get; set; }
        
    }

    
