require_relative 'scripts/coverage'
require_relative 'scripts/copy-dependencies'

task :copy_to_pksim do
	copy_to_app '../PK-Sim/src/PKSim/bin/Debug/'
end

task :copy_to_mobi do
	copy_to_app '../MoBi/src/MoBi/bin/Debug/'
end

private

def copy_to_app(app_target_relative_path)
  app_target_path = File.join(solution_dir, app_target_relative_path)
  source_dir = File.join(src_dir, 'OSPSuite.UI', 'bin', 'Debug')

  copy_depdencies source_dir,  app_target_path do
    copy_file 'OSPSuite.*.dll'
    copy_file 'OSPSuite.*.pdb'
  end

end

def solution_dir
	File.dirname(__FILE__)
end

def src_dir
	File.join(solution_dir, 'src')
end
	