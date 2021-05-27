#!/usr/bin/env python3
#essai de programmes de messagerie
import socket
import logging

# Configuration du logger pour une gestion des logs
logging.basicConfig(filename="server.log", filemode="w", 
        format="[%(asctime)s]%(name)s::%(levelname)s => %(message)s",
        level=logging.INFO
    )

# On instancie le socket client pour une eventuelle communication
connect = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
logging.info("socket instance created for client")
server = ''
try:
    port = int(input('port-serveur: '))
except:
    logging.warning("incorrect value for server port: 3800 chosen by default")
    port = 3800
# On instancie un socket pour le serveur proprement dit
connection = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
# On utilise la methode `bind` pour configurer le port et le domaine du serveur
connection.bind((server, port))
logging.info("waiting for connection.....")
# Le serveur se met alors en position d'ecoute
# l'entier passe en parametre definit le nombre de connections non
# acceptees par le serveur avant qu'il ne denie systematiquement tte
# tentative de connexion
connection.listen(5)
# Le serveur accepte une connexion avec la methode `accept`
# Il ne peut accepter qu'une seule connexion en mode bloquant
# Il existe d'autres methodes pour lui faire accepter plus
# d'une connexion
connect, info = connection.accept()
logging.info("connection established with %s", str(connect))
pseudo = input('pseudo:')
received = b""
sent = b""
logging.info("discussion started.")
# On peut alors envoyer des donnees vers le recepteur avec la methode `send`
# du socket instancie
connect.send(b"pseudo-interlocuteur:" + pseudo.encode())
# On traite certains cas d'exceptions:
# - lorsque la connexion est interrrompu de facon soudaine
# - ou lorsqu'il y a une erreur quelconque
try:
    while (not received.decode().endswith("end")) and sent != "end":
        # Recuperation du contenu provenant du correspondant avec `recv`
        received = connect.recv(1024)
        if received != b"":
            print(received.decode())
            sent = input('moi: ')
            connect.send(pseudo.encode() +b": " + sent.encode())
except BrokenPipeError:
    print("COnnection fermee par correspondant")
except:
    print("Une erreur est survenue")
finally:
    # On ferme la connexion apres arret de la communication
    connect.close()
    logging.info("Connection closed[<<EOF>>]")
    print('<<EOF>>')
