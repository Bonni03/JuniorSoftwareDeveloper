# TaskManagerAPI - Test Tecnico

## Obiettivo

Implementa una Minimal API in .NET 8 che consenta di gestire task multi-tenant, salvando i dati in un file JSON locale, con particolare attenzione alla sicurezza dei dati.

## Funzionalità richieste

- POST /tasks
- GET /tasks (con filtraggio tramite header `X-Tenant-ID`
- Salvataggio dei dati in file JSON

## Funzionalità facoltative

- PUT /tasks/{id}
- Test unitari

## Extra

- Per ogni task crea un record di dati nel nostro applicativo. 

Valorizza i seguenti campi (fieldName):

	1. TASK_ID (String 255)
	2. TASK_DESCRIPTION (String 255)
	3. CREATION DATE (Date)

Al seguente link trovi lo swagger: https://services.paloalto.swiss:10443/api2/swagger/index.html

## Istruzioni

- Puoi modificare la struttura del progetto come preferisci
- Usa solo file system (niente database)
- Inserisci le tue risposte nel file `README.md` alla fine

## Domande finali

1. Hai riscontrato difficoltà? Dove?
	Si, ho trovato difficolta nel completare l'Extra,
	soprattutto perche non riuscivo a capire dove inviare 
	il record di un nuovo task anche se quando ho capito dove 
	dovevo inviarlo ho notato che era abbastanza apparente.
2. Hai fatto assunzioni? Se sì, quali?
	Ho assunto che i task hanno un id unico e quindi due tasks non possono
	avere lo stesso id.
3. Come miglioreresti il codice se fosse un progetto reale?
	Sicuramente farei dei test unitari e costruire il codice in moduli 
	piu piccolo cosi da renderlo piu facile da maneggiare da me in futuro
	e altri developers
4. Hai usato strumenti di supporto (AI, StackOverflow, ecc)? Se sì, come?
 	Si ho usato StackOverflow, ChatGPT e la documentazione di .net, qua sotto spiego come:
  - StackOverflow e la doc di .net mi hanno principalmente aiutato principalmente per tutto quello
	che riguarda gestione dello swagger e httpclient
  - ChatGPT la ho usata per velocizzare il processo, cioe' se non mi ricordavo
	un specifico metodo chiedevo a ChatGPT siccome e' molto piu veloce che cercare
	nella documentazione e comunque puo dare un link alla documentazione dove posso
	leggere quello di cui ho bisogno piu nello specifico. Alla fine la ho anche usata 
	per fare una review finale del mio codice. 
