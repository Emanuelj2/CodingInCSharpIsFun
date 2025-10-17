#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define NPROC 4

struct proc {
	int pid;                // Process ID
	char name[16];
	char name[16];         // Process name
};

struct proc proc_table[NPROC]; // Process table
struct proc* init_proc;      // Initial process pointer

int main() {
	init_proc = &proc_table[0]; // Point to the first process
	init_proc->pid = 1;        // Set PID to 1
	strcpy(init_proc->name, "init");

	printf("Process ID: %d, Name: %s\n", init_proc->pid, init_proc->name);
	return 0;

}