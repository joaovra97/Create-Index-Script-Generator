var nfeIndexes = ["Number", "Client._id", "Emission", "Issuer", "ClientToOrder", "Category._id"];
var genericIndexes = ["Key", "Category._id", "Client._id"];

function GetIndexesJson(start, limit) {
    const dbs = db.getMongo().getDBNames();
    for (var i = start; i < dbs.length; i++) {
        if (i >= limit)
            break;

        if (dbs[i].search("QuestorEdoc") == -1 || (dbs[i].search("QuestorEdocAdmin") !== -1 || (dbs[i].search("QuestorEdocConfig") !== -1)))
            continue;

        var database = db.getMongo().getDB(dbs[i]);

        if (database.Company.count() == 0)
            continue;

        var nfeCollection = ListIndexes("Nfe", nfeIndexes.slice(), database);
        var nfceCollection = ListIndexes("Nfce", genericIndexes.slice(), database);
        var cteCollection = ListIndexes("Cte", genericIndexes.slice(), database);
        var cteosCollection = ListIndexes("Cteos", genericIndexes.slice(), database);
        var cfeCollection = ListIndexes("Cfe", genericIndexes.slice(), database);
        
        var stats = database.stats();        
        var company = database.Company.findOne();
        
        var row = {
            "db":stats.db,
            "company_name":company.Name,
            "federal_registration":company.FederalRegistration,
            "collections":[
                nfeCollection,
                nfceCollection,
                cteCollection,
                cteosCollection,
                cfeCollection
            ]
        }
        
        print(row);
    }
}

function ListIndexes(collection, indexesList, database) {    
    var indexes = database.getCollection(collection).getIndexes();
    indexes = indexes.filter(index => index.name != "_id_");
    
    const { expected, missed } = getExpectedIndex(indexes, indexesList)
    const notExpected = getNotExpectedIndex(indexes, expected)
    
    var collection = {
        "name":collection,
        "expected_indexes":expected,
        "missed_indexes":missed,
        "not_expected_indexes":notExpected
    };
    
    return collection;
}

function getExpectedIndex(indexDataTable, indexList) {
    const expected = []
    const missed = indexList;
    for (let j = 0; j < indexDataTable.length; j++) {
        for (let i = 0; i < missed.length; i++) {
            if (indexDataTable[j].name.includes(missed[i])) {
                expected.push(indexDataTable[j].name)
                if (indexDataTable[j].name !== "_id_")
                    missed.splice(i, 1)
                break;
            }
        }
    }
    return { expected, missed }
}

function getNotExpectedIndex(indexDataTable, expectedIndex) {
    const notExpected = []
    for (let i = 0; i < indexDataTable.length; i++) {
        let expected = false
        for (let j = 0; j < expectedIndex.length; j++) {
            if (indexDataTable[i].name.includes(expectedIndex[j])) {
                expected = true
                break;
            }
        }
        if (!expected)
            notExpected.push(indexDataTable[i].name)
    }
    return notExpected
}

GetIndexesJson(0, 5);