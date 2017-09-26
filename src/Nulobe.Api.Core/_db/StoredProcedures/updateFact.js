function updateFact(fact) {

    let context = getContext();
    let collection = context.getCollection();
    let response = context.getResponse();

    let factDocument = null;
    let linkedFactCount = null;
    let updatedLinkedFactCount = 0;

    let factQuery = `SELECT VALUE f FROM Facts f WHERE f.id = '${fact.id}'`;
    collection.queryDocuments(collection.getSelfLink(), factQuery, {}, (err, factDocuments) => {
        if (err) throw err;

        factDocument = factDocuments[0];
        if (factDocument.Title !== fact.title) {
            let linkedFactsQuery = `SELECT VALUE f FROM Facts f JOIN s IN f.Sources WHERE s.Type = 2 AND s.FactId = '${fact.id}'`;
            collection.queryDocuments(collection.getSelfLink(), linkedFactsQuery, {}, onLinkedFactsQuery);
        } else {
            replaceFactDocument(fact);
        }
    });

    function onLinkedFactsQuery(err, linkedFactDocuments) {
        if (err) throw err;

        linkedFactCount = linkedFactDocuments.length;
        if (linkedFactCount) {
            linkedFactDocuments.forEach(linkedFactDocument => {
                linkedFactDocument.Sources.forEach(source => {
                    if (source.FactId === fact.id) {
                        source.FactTitle = fact.Title;
                    }
                });
                collection.replaceDocument(linkedFactDocument._self, linkedFactDocument, onLinkedFactUpdated);
            });
        } else {
            replaceFactDocument();
        }
    }

    function onLinkedFactUpdated(err, linkedFactDocument) {
        if (err) throw err;

        updatedLinkedFactCount++;
        if (updatedLinkedFactCount === linkedFactCount) {
            replaceFactDocument();
        }
    }

    function replaceFactDocument() {
        collection.replaceDocument(factDocument._self, fact, (err, updatedFact) => {
            if (err) throw err;
            response.setBody(updatedFact);
        });
    }
}