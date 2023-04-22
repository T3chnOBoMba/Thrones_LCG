using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Linq;


namespace database{
    public class GameDatabase{

        private static string dbName = "URI=file:GameDatabase.sqlite";

        private static IDbConnection GetConnection(){
            IDbConnection connection = new SqliteConnection(dbName);
            connection.Open();

            // create database tables if they do not already exist
            IDbCommand commandCreateCardsTable = connection.CreateCommand();
            commandCreateCardsTable.CommandText = $@"CREATE TABLE IF NOT EXISTS cards
            (code TEXT PRIMARY KEY NOT NULL,
            name TEXT NOT NULL,
            card_type TEXT NOT NULL,
            cost INTEGER,
            traits TEXT,
            faction TEXT NOT NULL,
            image_name TEXT NOT NULL,
            card_text TEXT,
            deck_limit INTEGER,
            ability_implimented INTEGER,
            military INTEGER,
            intrigue INTEGER,
            power INTEGER,
            loyal INTEGER,
            strength INTEGER,
            initiative INTEGER,
            claim INTEGER,
            reserve INTEGER,
            income INTEGER
            pack_name TEXT)";
         
            // if table is empty, fill with card data
            IDbCommand commandCheckIfExists = connection.CreateCommand();
            commandCheckIfExists.CommandText = "SELECT count(*) from cards";
            IDataReader reader = commandCreateCardsTable.ExecuteReader();
            if(!reader.Read()){
                populateDatabase("Assets/Scripts/Cards/thrones_card_data.json");
            }


            return connection;
        }

        private static void populateDatabase(string path){
            using (StreamReader r = new StreamReader(path)){
                string json =  r.ReadToEnd();
                List<CardDataContainer> cards = JsonConvert.DeserializeObject<List<CardDataContainer>>(json);
                foreach(CardDataContainer card in cards){
                    if(!card.work_in_progress && !String.Equals(card.pack_name, "Redesigns")){
                        addCard(card);
                    }              
                }
            }
        }

        //@ToDo
       private static bool addCard(CardDataContainer card){
            IDbConnection connection = GetConnection();
            IDbCommand commandAddCard = connection.CreateCommand();
            string imageFileName = card.name.Replace(" ", "_") + $"_{card.code}.jpg";
            try{
                commandAddCard.CommandText = $@"INSERT INTO cards (
                code,
                name,
                card_type,
                cost,
                traits,
                faction,
                image_name,
                card_text,
                deck_limit,
                ability_implimented,
                military,
                intrigue,
                power,
                loyal,
                strength,
                initiative,
                claim
                reserve,
                income,
                pack_name) VALUES (
                    {card.code},
                    {card.name},
                    {card.type_code},
                    {card.traits},
                    {card.faction_code},
                    {imageFileName},
                    {card.text},
                    {card.deck_limit},
                    0,
                    {card.is_military},
                    {card.is_intrigue},
                    {card.is_power},
                    {card.is_loyal},
                    {card.strength},
                    {card.initiative},
                    {card.claim},
                    {card.reserve},
                    {card.income},
                    {card.pack_name}
                )";
                commandAddCard.ExecuteNonQuery();
            }
            catch(Exception e){
                Debug.Log("Failed to save card: " + e);
                return false;
            }
            
            // download image for card in 'faction name'/'card_name'_'card_code'
            try{
                using (WebClient webClient = new WebClient()) {
                webClient.DownloadFile(card.image_url, $"Assets/Resources/{card.faction_code}/{card.type_code}/{imageFileName}"); 
                }
            }
            catch(Exception e){
                Debug.Log("Failed to save image: " + e);
                return false;
            }
            
            return true;
       }

        //@ToDo
        public static List<Card> getCards(string faction="", string type="", string name="",
        List<string> factions=null){
            IDbConnection connection = GetConnection();
            IDbCommand commandGet = connection.CreateCommand();
            string commandString = "SELECT * FROM cards";

            // build 'where' clause

            if(!string.IsNullOrEmpty(faction) || !string.IsNullOrEmpty(type) ||
                !string.IsNullOrEmpty(name) || factions != null){
                    commandString += " WHERE ";
                }

            bool firstOpInString = false;   // first 'where' clause is in commandString

            if(factions != null){
                commandString += $"(faction IN ({string.Join(",", factions)}))";
                firstOpInString = true;
            }

            if(!string.IsNullOrEmpty(faction)){
                if(firstOpInString){
                    commandString += " AND ";
                }
                commandString += $"(faction={faction})";
            }

            if(!string.IsNullOrEmpty(type)){
                if(firstOpInString){
                    commandString += " AND ";
                }
                commandString += $"(card_type={type})";
            }

            if(!string.IsNullOrEmpty(name)){
                if(firstOpInString){
                    commandString += " AND ";
                }
                commandString += $"(name={name})";
            }

            commandGet.CommandText = commandString;
            List<Card> cards = new List<Card>();
            try{
                using(IDataReader reader = commandGet.ExecuteReader()){
                while(reader.Read()){

                    cards.Add(new Card(
                        Name: reader["name"].ToString(),
                        CardType: reader["card_type"].ToString(),
                        Faction: reader["faction"].ToString(),
                        Code: reader["code"].ToString(),
                        Cost: getNullibleInt(reader["cost"].ToString()),
                        Traits: reader["traits"].ToString().Split(",").ToList(),
                        Image: Resources.Load<Sprite>(reader["image_name"].ToString()),
                        Text: reader["text"].ToString(),
                        DeckLimit: (int)getNullibleInt(reader["deck_limit"].ToString()),
                        AbilityImplimented: (bool)getBoolFromString(reader["ability_implimented"].ToString()),
                        Military: getBoolFromString(reader["military"].ToString()),
                        Intrigue: getBoolFromString(reader["intrigue"].ToString()),
                        Power: getBoolFromString(reader["power"].ToString()),
                        Loyal: getBoolFromString(reader["loyal"].ToString()),
                        Strength: getNullibleInt(reader["strength"].ToString()),
                        Income: getNullibleInt(reader["income"].ToString()), 
                        Initiative: getNullibleInt(reader["initiative"].ToString()),
                        Claim: getNullibleInt(reader["claim"].ToString()),
                        Reserve: getNullibleInt(reader["reserve"].ToString())
                    ));
                    }
                }
                return cards;
            }
            catch(Exception e){
                Debug.Log("Error retrieving cards: " + e);
                return null;
            }
        }

        private static bool? getBoolFromString(string id){
            int number;
            return Int32.TryParse(id, out number) ? (number == 0 ? false : true) : null;
        }

        private static int? getNullibleInt(string id){
            int number;
            return Int32.TryParse(id, out number) ? number : null;
        }
    }

    [Serializable]
    public class CardDataContainer{
        public string code;
        public string name;
        public string type_code;
        public string traits;
        public string faction_code;
        public string text;
        public int deck_limit;
        public bool is_military;
        public bool is_intrigue;
        public bool is_power;
        public bool is_loyal;
        public int strength;
        public int initiative;
        public int claim;
        public int reserve;
        public int income;
        public string pack_name;
        public bool work_in_progress;
        public string image_url;
    }

}

