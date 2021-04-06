#Cahier de charges :
Le but de l'exercice est de faire une REST API pour de la gestion d'evenement.
Il doit etre possible d'ajouter, modifier, supprimer et visualiser des evenements (sous forme de liste et de manière individuel).
Je peux gérer des commentaires pour un evenements (création, modification, suppression, visualisation).
Pour le retour de la liste des evenements, chaque evenement doit contenir sa liste de commentaires.
La liste des événements doit permettre la pagination.
Les migrations de database doivent etre faite avec Entity Framework Core.
Les retours doivent etre de type JSON.

Un évènement est constitué d'un titre de 100 caracteres maximum, d'une description et d'une personne impliquée dans l'évènement.
Un commentaire est constitué d'une description et d'une date et est obligatoirement relié à un evenement.

#Fournis :
	Projet de base sans controlleur avec Context (complet + migration auto) et docker de setup avec un postgresql