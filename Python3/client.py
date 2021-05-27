#!/usr/bin/env python3
# importation des modules necessaires
import socket
import logging

# configuration des logs (histoire de suivre l'evolution de l'app)
logging.basicConfig(filename="client.log", filemode="w", 
        format="[%(asctime)s]%(name)s::%(levelname)s => %(message)s",
        level=logging.INFO
    )
logging.info("socket instance created\n")
# Instantiation du socket a utiliser pour la connexion
connect = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
host = input('ip-serveur: ')
s_port: int = 0;
try:
    s_port = int(input('port-serveur: '))
except:
    logging.warning("Invalid port value, defaulted to 3000.")
    s_port = 3000
logging.info("Attempting to connect to server %s:%d\n", host, s_port)
# Tentative de connexion avec la methode `connect`
# Elle est bloquante, le programme n'évolue donc que si elle s'acheve.
# Et elle peut soit reussir soit echouer
connect.connect((host, s_port))
logging.info("Connection succeeded\n")
pseudo = input('Pseudo:')
received = b""
sent = b""
# On peut alors envoyer des donnees vers le recepteur avec la methode `send`
# du socket instancie
connect.send(b"Pseudo-interlocuteur:" + pseudo.encode())
logging.info("Discussion started\n")
# On traite certains cas d'exceptions:
# - lorsque la connexion est interrrompu de facon soudaine
# - ou lorsqu'il y a une erreur quelconque
try:
    while (not received.decode().endswith("end")) and sent != "end":
        # Recuperation du contenu provenant du correspondant avec `recv`
        received = connect.recv(1024)
        if received != b"":
            print(received.decode())
            sent = input('Moi: ')
            connect.send(pseudo.encode() +b": " + sent.encode())
except BrokenPipeError:
    print("Connection fermée par le correspondant")
except:
    print("Une erreur est survenue!")
finally:
    # On ferme la connexion apres arret de la communication
    connect.close()
    logging.info("COnnection closed[<<EOF>>]\n")
    print('<<EOF>>')
