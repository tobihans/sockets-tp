#include <QCoreApplication>
#include <QString>
#include <QTextStream>
#include <QHostAddress>
#include <QUdpSocket>

int main(int argc, char *argv[])
{
    const int PORT = 5000;
    // Chaine qui marque la fin de la communication
    const QString EOC{"<EOF>"};
    auto socket = new QUdpSocket();
    // Connexion en local sur le port défini
    socket->connectToHost(QHostAddress::LocalHostIPv6,PORT);
    // On attend pendant 45s une conenxion entrante
    if(socket->waitForConnected(45000))
    {
        while (true) // Boucle principale du progamme
        {
            QTextStream out{stdout};
            out << "> ";
            // On lit a travers l'entrée standard pour envoyer notre message
            QTextStream in{stdin};
            const QString msg = in.readLine();
            socket->write(msg.toLatin1());
            if(socket->waitForBytesWritten(1000))
            {
                qDebug() << "Message envoyé";
                if(socket->waitForReadyRead())
                {
                    const QString response{socket->readAll()};
                    // Lorsqu'on recoit la chaine de fin de communication, on quitte la boucle
                    if(response.indexOf(EOC) > -1) break;
                    qDebug() << "Réponse : " << response;
                }
                else
                {
                    qDebug() << "Problème de lecture";
                    qDebug() << "Problème d'écriture";
                    exit (0);
                }
            }
            else
            {
                qDebug() << "Lecture impossible";
                exit (0);
            }
            out.flush();
            in.flush();
        }
    }
    return 0;
}