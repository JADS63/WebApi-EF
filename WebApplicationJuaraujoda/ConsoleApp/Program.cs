// See https://aka.ms/new-console-template for more information
using WebApiUtilisation;
using Dto;



// La méthode Get Testée 
string BaseApiUrlPlayersGet = @"https://codefirst.iut.uca.fr/containers/mchSamples_NET-my_favorite_things_api/Albums";
string RouteGetAlbums = "{0}?index={1}&count={2}";
string route = string.Format(RouteGetAlbums, BaseApiUrlPlayersGet, 0, 5);
IEnumerable<PlayerDto> result = await GetFromRoute<IEnumerable<PlayerDto>>(route);

// La méthode Post Testée 
string BaseApiUrlPlayersPost = @"https://codefirst.iut.uca.fr/containers/mchSamples_NET-my_favorite_things_api/Albums";
PlayerDto item = new PlayerDto
{
    Id = 100,
    FirstName = "David Murray",
    BirthDate = new DateTime(2024, 5, 17),
    Height = 175,
    HandPlay = HandPlay.None,
    LastName = "Test",
    Nationality = "usa"
};
PlayerDto? inserted = await PostItemAsync(BaseApiUrlPlayersPost, item);

// La méthode Put Testée 
string BaseApiUrlPlayersPut = @"https://codefirst.iut.uca.fr/containers/mchSamples_NET-my_favorite_things_api/Albums";
string RoutePutPlayers = "{0}?id={1}";
string route2 = string.Format(RoutePutPlayers, BaseApiUrlPlayersPut, 9);
PlayerDto item2 = new PlayerDto
{
    Id = 100,
    FirstName = "David Murray",
    BirthDate = new DateTime(2024, 5, 17),
    Height = 175,
    HandPlay = HandPlay.None,
    LastName = "Test",
    Nationality = "usa"
};
PlayerDto? updated = await PutItemAsync(route2, item2);


// La méthode Delete Testée 

string BaseApiUrlPlayersDelete = @"https://codefirst.iut.uca.fr/containers/mchSamples_NET-my_favorite_things_api/Albums";
string RouteDeletePlayers = "{0}?id={1}";
string route3 = string.Format(RouteDeletePlayers, BaseApiUrlPlayersDelete, 9);
bool result2 = await DeleteItemAsync(route3);