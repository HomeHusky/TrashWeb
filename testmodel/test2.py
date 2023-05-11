import socket
import base64
import json
import os
import time
import threading

class ModelTrash:
    def __init__(self):
        self.data = {
                    'Recycle': 1,
                    'Dangerous': 2,
                    'Other Garbage': 3,
                    }
        self.create_folder = 0
        self.save_images = 0
        self.data_recv = ""
        
    def update_trash_list(self, name): 
        """
        This function updates the count of a specific item in a trash list.
        
        :param name: The name of the item that needs to be updated in the trash list
        """
        self.data[name] += 1

    def get_values(self):
        """
        This function returns a list of all the values in a dictionary.
        :return: The function `get_values` is returning a list of all the values in the dictionary
        `self.data`.
        """
        list_temp = []
        for value in self.data.values():
            list_temp.append(value)
        return list_temp
    
    # def run_predict(self):
        

    def data_server(self, data):
        host2 = '192.168.1.7'
        port2 = 5565
        # send data to next server
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((host2, port2))
            print("Connect success!")
            s.sendall(str(data).encode())
            data_recv = s.recv(1024)
            if len(data_recv):
                response = data_recv.decode()
                self.data_recv = response
                print("Receive from server >> ", response)
                if response == "sendstring":
                    s.sendall(str(data).encode())
                elif response == "sendlist":
                    s.send(str("sendlist").encode())
                elif response == "Delete":    
                    pass
                else:
                    pass
                print(data_recv, "\n", self.save_images,"\n=========\n")

    def start_server(self):
        while True:
            print("=======================================")
            list_temp = self.get_values()
            self.data_server(list_temp)
            time.sleep(1)

ModelTrash.start_server(self=None)