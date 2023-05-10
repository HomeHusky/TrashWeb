import time 
person = True
start_person = time.time()
status_in = False
status_out = False
count = 0
allow_detect_trash = False
start = time.time()
while True:
    end_person = time.time()
    if person:
        if end_person-start_person >= 4:
            person = False
            start_person = time.time()

    if person == False:
        end_person = time.time()
        print(end_person-start_person)
        if end_person-start_person >= 4:
            person = True
            print("test")
    
    print(person)
    
    # if person:
    #     if status_in==False:
    #         start_in = time.time()
    #         status_in = True
    #     else:
    #         end_in = time.time()
            
        
    #     if end_in-start_in >= 2:
    #         allow_detect_trash = True
    # else:
    #     if allow_detect_trash:
    #         if status_out == False:
    #             start_out = time.time()
    #         status_out = True

    #     if status_out:
    #         end_out = time.time()

    #     if end_out-start_out >=2:
    #         allow_detect_trash = False
    
    # print(allow_detect_trash)

    if time.time()- start >= 16:
        break

        
    