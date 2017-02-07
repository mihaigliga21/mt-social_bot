from subprocess import Popen, PIPE, STDOUT
import sys

def run_command(phrase):
    cmd = 'java -jar C:\mt-bot\WordAnalyser.jar ' + phrase
    p = Popen(cmd, stdout=PIPE, stderr=STDOUT, shell=True)
    rt = p.stdout
    cmd_result = rt.read()
    print(cmd_result)
    return cmd_result    


arg = sys.argv[1]
run_command(arg)