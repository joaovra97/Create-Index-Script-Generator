var database = null;

database = db.getMongo().getDB('QuestorEdoc00124424961');
database.getCollection('Nfe').createIndex({ ClientToOrder: 1 },{ background: true})

database = db.getMongo().getDB('QuestorEdoc01750023000186');
database.getCollection('Nfe').createIndex({ ClientToOrder: 1 },{ background: true})

database = db.getMongo().getDB('QuestorEdoc77958346000121');
database.getCollection('Nfe').createIndex({ ClientToOrder: 1 },{ background: true})


//Indexes para adicionar no arquivo de instalação de bases do Zen
//CreateIndex('Nfe', 'Emission_-1');
//CreateIndex('Nfce', 'DhEmi');
//CreateIndex('Cte', 'DhEmi_-1');
//CreateIndex('Cfe', 'DateEmission_1');
//CreateIndex('Cte', 'DhEmi_1');
